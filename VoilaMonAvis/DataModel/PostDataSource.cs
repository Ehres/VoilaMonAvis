using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VoilaMonAvis;
using VoilaMonAvis.DataAccessLayer;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace VoilaMonAvis.DataModel
{
    [Windows.Foundation.Metadata.WebHostHidden]
    public class PostDataCommon : VoilaMonAvis.Common.BindableBase
    {
        private static Uri _baseUri = new Uri("ms-appx:///");

        public PostDataCommon(String uniqueId, String title, String imagePath)
        {
            this._uniqueId = uniqueId;
            this._title = title;
            this._imagePath = imagePath;
        }

        private string _uniqueId = string.Empty;
        public string UniqueId
        {
            get { return this._uniqueId; }
            set { this.SetProperty(ref this._uniqueId, value); }
        }

        private string _title = string.Empty;
        public string Title
        {
            get { return this._title; }
            set { this.SetProperty(ref this._title, value); }
        }

        private ImageSource _image = null;
        private String _imagePath = null;
        
        public ImageSource Image
        {
            get
            {
                if (this._image == null && this._imagePath != null)
                {
                    this._image = new BitmapImage(new Uri(_baseUri, this._imagePath));
                }
                return this._image;
            }

            set
            {
                this._imagePath = null;
                this.SetProperty(ref this._image, value);
            }
        }

        public void SetImage(String path)
        {
            this._image = null;
            this._imagePath = path;
            this.OnPropertyChanged("Image");
        }
        
        public override string ToString()
        {
            return this.Title;
        }
    }


    /// <summary>
    /// Modèle de données d'élément générique.
    /// </summary>
    public class PostDataItem : PostDataCommon
    {
        public PostDataItem(String uniqueId, String title, String imagePath, String content, PostDataGroup group, Uri videoPath, int page)
            : base(uniqueId, title, imagePath)
        {
            this.Content = content;
            this.Group = group;
            this.VideoPath = videoPath;
            this.Page = page;
        }

        public string Content { get; set; }
        public string ContentNext { get; set; }
        public PostDataGroup Group { get; set; }
        public Uri VideoPath { get; set; }
        public int Page { get; set; }
    }


    /// <summary>
    /// Modèle de données de groupe générique.
    /// </summary>
    public class PostDataGroup : PostDataCommon
    {
        public PostDataGroup(String uniqueId, String title, String imagePath, int pageCount)
            : base(uniqueId, title, imagePath)
        {
            this.PageCount = pageCount;
            Items.CollectionChanged += ItemsCollectionChanged;
        }

        public int PageCount { get; set; }
        private void ItemsCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            // Fournit un sous-ensemble de la collection complète d'éléments avec laquelle effectuer une liaison à partir d'un GroupedItemsPage
            // pour deux raisons : GridView ne virtualise pas les collections d'éléments volumineuses, et
            // améliore l'expérience utilisateur lors de la navigation dans des groupes contenant un nombre important
            // d'éléments.
            //
            // 12 éléments maximum sont affichés, car cela se traduit par des colonnes de grille remplies,
            // qu'il y ait 1, 2, 3, 4 ou 6 lignes affichées

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if (e.NewStartingIndex < 5)
                    {
                        TopItems.Insert(e.NewStartingIndex,Items[e.NewStartingIndex]);
                        if (TopItems.Count > 5)
                        {
                            TopItems.RemoveAt(5);
                        }
                    }
                    break;
                case NotifyCollectionChangedAction.Move:
                    if (e.OldStartingIndex < 5 && e.NewStartingIndex < 5)
                    {
                        TopItems.Move(e.OldStartingIndex, e.NewStartingIndex);
                    }
                    else if (e.OldStartingIndex < 5)
                    {
                        TopItems.RemoveAt(e.OldStartingIndex);
                        TopItems.Add(Items[4]);
                    }
                    else if (e.NewStartingIndex < 5)
                    {
                        TopItems.Insert(e.NewStartingIndex, Items[e.NewStartingIndex]);
                        TopItems.RemoveAt(5);
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    if (e.OldStartingIndex < 5)
                    {
                        TopItems.RemoveAt(e.OldStartingIndex);
                        if (Items.Count >= 5)
                        {
                            TopItems.Add(Items[4]);
                        }
                    }
                    break;
                case NotifyCollectionChangedAction.Replace:
                    if (e.OldStartingIndex < 5)
                    {
                        TopItems[e.OldStartingIndex] = Items[e.OldStartingIndex];
                    }
                    break;
                case NotifyCollectionChangedAction.Reset:
                    TopItems.Clear();
                    while (TopItems.Count < Items.Count && TopItems.Count < 5)
                    {
                        TopItems.Add(Items[TopItems.Count]);
                    }
                    break;
            }
        }

        private ObservableCollection<PostDataItem> _items = new ObservableCollection<PostDataItem>();
        public ObservableCollection<PostDataItem> Items
        {
            get { return this._items; }
        }

        private ObservableCollection<PostDataItem> _topItem = new ObservableCollection<PostDataItem>();
        public ObservableCollection<PostDataItem> TopItems
        {
            get {return this._topItem; }
        }
    }

    /// <summary>
    /// Crée une collection de groupes et d'éléments dont le contenu est codé en dur.
    /// 
    /// PostDataSource initialise avec les données des espaces réservés plutôt que les données de production
    /// actives, afin que les exemples de données soient fournis à la fois au moment de la conception et de l'exécution.
    /// </summary>
    public sealed class PostDataSource
    {
        private static PostDataSource _postDataSource = new PostDataSource();

        private ObservableCollection<PostDataGroup> _allGroups = new ObservableCollection<PostDataGroup>();
        public ObservableCollection<PostDataGroup> AllGroups
        {
            get { return this._allGroups; }
        }

        public static IEnumerable<PostDataGroup> GetGroups(string uniqueId)
        {
            if (!uniqueId.Equals("AllGroups")) throw new ArgumentException("Only 'AllGroups' is supported as a collection of groups");

            return _postDataSource.AllGroups;
        }

        public static PostDataGroup GetGroup(string uniqueId)
        {
            // Une simple recherche linéaire est acceptable pour les petits groupes de données
            var matches = _postDataSource.AllGroups.Where((group) => group.UniqueId.Equals(uniqueId));
            if (matches.Count() == 1) return matches.First();
            return null;
        }

        public static PostDataItem GetItem(string uniqueId)
        {
            // Une simple recherche linéaire est acceptable pour les petits groupes de données
            var matches = _postDataSource.AllGroups.SelectMany(group => group.Items).Where((item) => item.UniqueId.Equals(uniqueId));
            if (matches.Count() == 1) return matches.First();
            return null;
        }

        public PostDataSource()
        {
            GetAllCategoriesAsync(false);            
        }

        private async void GetAllCategoriesAsync(bool reload)
        {
            #region Recents posts
            if (!reload)
            {
                List<Posts> recentPosts = await PostsDal.GetRecentPost();

                var groupRecentPost = new PostDataGroup("0",
                       "Derniers articles",
                       "Assets/DarkGray.png",
                       1);
                foreach (Posts post in recentPosts)
                {
                    string content = WebUtility.HtmlDecode(Regex.Replace(post.Post_Content, @"<[^>]*>", String.Empty));
                    string title = WebUtility.HtmlDecode(Regex.Replace(post.Post_Title, @"<[^>]*>", String.Empty));

                    groupRecentPost.Items.Add(new PostDataItem("Derniers articles" + "-Item-" + post.ID,
                        title,
                        post.Post_Image_Full_Url,
                        content,
                        groupRecentPost,
                        post.Post_Video_Url,
                        1));
                }
                this.AllGroups.Add(groupRecentPost);
            }
            #endregion

            #region All Categories and posts
            //List<Category> Categories = await CategoryDal.GetAllCategories();
            List<Task<List<Posts>>> tasks = new List<Task<List<Posts>>>();
            List<Category> Categories = await CategoryDal.GetMainCategories();

            foreach (Category category in Categories)
            {
                var groupTest = GetGroup(category.Category_Id.ToString());
                if (groupTest == null)
                {
                    var group = new PostDataGroup(category.Category_Id.ToString(),
                        category.Category_Title,
                        "Assets/DarkGray.png",
                        1);
                    List<Posts> posts = await PostsDal.GetPostsByCategory(category.Category_Id.ToString());
                    tasks.Add(PostsDal.GetPostsByCategory(category.Category_Id.ToString()));
                    
                    

                    foreach (Posts post in posts)
                    {
                        string content = WebUtility.HtmlDecode(Regex.Replace(post.Post_Content, @"<[^>]*>", String.Empty));
                        //string contentNext = WebUtility.HtmlDecode(Regex.Replace(post.Post_Content_Next, @"<[^>]*>", String.Empty));
                        string title = WebUtility.HtmlDecode(Regex.Replace(post.Post_Title, @"<[^>]*>", String.Empty));
                        group.Items.Add(new PostDataItem(category.Category_Id.ToString() + "-Item-" + post.ID,
                            title,
                            post.Post_Image_Full_Url,
                            content,
                            group,
                            post.Post_Video_Url,
                            1));
                    }
                    this.AllGroups.Add(group);
                }
            }     
            #endregion       

            
        }

        private static bool bCancelGetMoreItemGroup = false;
        public static void CancelGetMoreItemGroup()
        {
            bCancelGetMoreItemGroup = true;
        }
        public static async Task<bool> GetMoreItemGroup(string uniqueId)
        {
            bool needItemLoad = true;
            int pageToLoad = 0;

            while (needItemLoad)
            {
                if (!bCancelGetMoreItemGroup)
                {
                    var group = GetGroup(uniqueId);
                    pageToLoad = group.Items.Last().Page + 1;

                    List<Posts> morePost;
                    if (uniqueId == "0")
                        morePost = await PostsDal.GetRecentPost(pageToLoad);
                    else
                        morePost = await PostsDal.GetPostsByCategory(uniqueId, pageToLoad);
                    

                    if (morePost != null && morePost.Count > 0)
                    {
                        foreach (Posts post in morePost)
                        {
                            string content = WebUtility.HtmlDecode(Regex.Replace(post.Post_Content, @"<[^>]*>", String.Empty));
                            //string contentNext = WebUtility.HtmlDecode(Regex.Replace(post.Post_Content_Next, @"<[^>]*>", String.Empty));
                            string title = WebUtility.HtmlDecode(Regex.Replace(post.Post_Title, @"<[^>]*>", String.Empty));
                            group.Items.Add(new PostDataItem(uniqueId + "-Item-" + post.ID,
                                title,
                                post.Post_Image_Full_Url,
                                content,
                                group,
                                post.Post_Video_Url,
                                pageToLoad));
                        }
                    }
                    else
                        needItemLoad = false;
                }
                else
                {
                    bCancelGetMoreItemGroup = false;
                    needItemLoad = false;
                }
            }
            return true;
        }

        public static async Task<List<PostDataItem>> Find(string query)
        {
            List<PostDataItem> items = _postDataSource.AllGroups.SelectMany(group => group.Items).Where((item) => item.Content.Contains(query) || item.Title.Contains(query)).ToList();

            List<Posts> posts = await PostsDal.Find(query);

            foreach (Posts post in posts)
            {
                var group = new PostDataGroup(post.Post_Categories.First().Category_Id.ToString(),
                        post.Post_Categories.First().Category_Title,
                        "Assets/DarkGray.png",
                        1);

                if (_postDataSource._allGroups.Where(g => g.UniqueId == group.UniqueId).Count() == 0)
                    _postDataSource._allGroups.Add(group);
                else
                    group = GetGroup(post.Post_Categories.First().Category_Id.ToString());

                string content = WebUtility.HtmlDecode(Regex.Replace(post.Post_Content, @"<[^>]*>", String.Empty));
                string title = WebUtility.HtmlDecode(Regex.Replace(post.Post_Title, @"<[^>]*>", String.Empty));
                PostDataItem pdi = new PostDataItem(post.Post_Categories.First().Category_Id + "-Item-" + post.ID,
                                title,
                                post.Post_Image_Full_Url,
                                content,
                                group,
                                post.Post_Video_Url,
                                1);
                group.Items.Add(pdi);

                if (!items.Contains(pdi))
                    items.Add(pdi);
                
            }
            if (items == null)
                items = new List<PostDataItem>();
                
            
            return items;            
        }
    }
}
