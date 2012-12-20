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

namespace VoilaMonAvis.Data
{
    public class Posts
    {
        public int ID { get; set; }
        public Users Post_Author { get; set; }
        public DateTime Post_Date { get; set; }
        public DateTime Post_Date_GMT { get; set; }
        public string Post_Content { get; set; }
        public string Post_Title { get; set; }
        public string Post_Excerpt { get; set; }
        public string Post_Status { get; set; }
        public string Comment_Status { get; set; }
        public string Ping_Status { get; set; }
        public string Post_Password { get; set; }
        public DateTime Post_Modified { get; set; }
        public string Post_Type { get; set; }
        public string Post_Mime_Type { get; set; }
        public int Comment_Count { get; set; }
        public string Post_Url { get; set; }
        public List<Category> Post_Categories { get; set; }
        public BitmapImage Post_Image_Thumbnail { get; set; }
        public BitmapImage Post_Image_Medium { get; set; }
        public BitmapImage Post_Image_Large { get; set; }
        public BitmapImage Post_Image_Full { get; set; }
        public string Post_Image_Full_Url { get; set; }

        public Posts(string json)
        {
            JObject jObject = JObject.Parse(json);
            //JToken jPost = jObject["posts"];
            ID = (int)jObject["id"];
            Post_Type = (string)jObject["type"];
            Post_Url = (string)jObject["url"];
            Post_Status = (string)jObject["status"];
            Post_Title = (string)jObject["title"];
            Post_Content = (string)jObject["content"];
            Post_Author = new Users(jObject["author"].ToString());
            Post_Url = (string)jObject["url"];
            Post_Status = (string)jObject["status"];
            Post_Excerpt = (string)jObject["excerpt"];
            Post_Date = (DateTime)jObject["date"];
            Post_Modified = (DateTime)jObject["modified"];          

            Post_Categories = new List<Category>();
            JToken jTokencategories = jObject["categories"];

            //JObject o = JObject.Parse(jTokencategories.ToString());
            foreach (var categoryJson in jTokencategories)
            {
                Category category = new Category(categoryJson.ToString());
                Post_Categories.Add(category);
            }

            JToken jTokenAttachments = jObject["attachments"];
            foreach (var attachmentJson in jTokenAttachments)
            {
                JObject jObjectAttachmentJson = (JObject)(attachmentJson);
                JObject jObjectImages = (JObject)jObjectAttachmentJson["images"];
                JToken jObjectImageFull = jObjectImages["full"];
                //Post_Image_Full = new BitmapImage((Uri)jObjectImageFull["url"]);
                Post_Image_Full_Url = jObjectImageFull["url"].ToString();
            }
        }

        public Posts() { }        
    }

    [Windows.Foundation.Metadata.WebHostHidden]
    public class PostDataCommon : VoilaMonAvis_FromScratch_.Common.BindableBase
    {
        private static Uri _baseUri = new Uri("ms-appx:///");

        public PostDataCommon(String uniqueId, String title, String subtitle, String imagePath, String description)
        {
            this._uniqueId = uniqueId;
            this._title = title;
            this._subtitle = subtitle;
            this._description = description;
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

        private string _subtitle = string.Empty;
        public string Subtitle
        {
            get { return this._subtitle; }
            set { this.SetProperty(ref this._subtitle, value); }
        }

        private string _description = string.Empty;
        public string Description
        {
            get { return this._description; }
            set { this.SetProperty(ref this._description, value); }
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
        public PostDataItem(String uniqueId, String title, String subtitle, String imagePath, String description, String content, PostDataGroup group)
            : base(uniqueId, title, subtitle, imagePath, description)
        {
            this._content = content;
            this._group = group;
        }

        private string _content = string.Empty;
        public string Content
        {
            get { return this._content; }
            set { this.SetProperty(ref this._content, value); }
        }

        private PostDataGroup _group;
        public PostDataGroup Group
        {
            get { return this._group; }
            set { this.SetProperty(ref this._group, value); }
        }
    }


    /// <summary>
    /// Modèle de données de groupe générique.
    /// </summary>
    public class PostDataGroup : PostDataCommon
    {
        public PostDataGroup(String uniqueId, String title, String subtitle, String imagePath, String description)
            : base(uniqueId, title, subtitle, imagePath, description)
        {
            Items.CollectionChanged += ItemsCollectionChanged;
        }

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
                    if (e.NewStartingIndex < 12)
                    {
                        TopItems.Insert(e.NewStartingIndex,Items[e.NewStartingIndex]);
                        if (TopItems.Count > 12)
                        {
                            TopItems.RemoveAt(12);
                        }
                    }
                    break;
                case NotifyCollectionChangedAction.Move:
                    if (e.OldStartingIndex < 12 && e.NewStartingIndex < 12)
                    {
                        TopItems.Move(e.OldStartingIndex, e.NewStartingIndex);
                    }
                    else if (e.OldStartingIndex < 12)
                    {
                        TopItems.RemoveAt(e.OldStartingIndex);
                        TopItems.Add(Items[11]);
                    }
                    else if (e.NewStartingIndex < 12)
                    {
                        TopItems.Insert(e.NewStartingIndex, Items[e.NewStartingIndex]);
                        TopItems.RemoveAt(12);
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    if (e.OldStartingIndex < 12)
                    {
                        TopItems.RemoveAt(e.OldStartingIndex);
                        if (Items.Count >= 12)
                        {
                            TopItems.Add(Items[11]);
                        }
                    }
                    break;
                case NotifyCollectionChangedAction.Replace:
                    if (e.OldStartingIndex < 12)
                    {
                        TopItems[e.OldStartingIndex] = Items[e.OldStartingIndex];
                    }
                    break;
                case NotifyCollectionChangedAction.Reset:
                    TopItems.Clear();
                    while (TopItems.Count < Items.Count && TopItems.Count < 12)
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
            getRecentPosts();            
        }

        private async void getRecentPosts()
        {
            #region Recents posts
            List<Posts> recentPosts = await PostsDal.GetRecentPost();

            var groupRecentPost = new PostDataGroup("0",
                   "Derniers articles",
                   "",
                   "Assets/DarkGray.png",
                   "");
            foreach (Posts post in recentPosts)
            {
                string content = WebUtility.HtmlDecode(Regex.Replace(post.Post_Content, @"<[^>]*>", String.Empty));
                string title = WebUtility.HtmlDecode(Regex.Replace(post.Post_Title, @"<[^>]*>", String.Empty));
                
                groupRecentPost.Items.Add(new PostDataItem("Derniers articles" + "-Item-" + post.ID,
                    title,
                    "",
                    post.Post_Image_Full_Url,
                    "",
                    content,
                    groupRecentPost));
            }
            this.AllGroups.Add(groupRecentPost);
            #endregion

            #region All Categories and posts
            List<Category> Categories = await CategoryDal.GetAllCategories();

            foreach (Category category in Categories)
            {
                var group = new PostDataGroup(category.Category_Id.ToString(),
                    category.Category_Title,
                    "",
                    "Assets/DarkGray.png",
                    category.Category_Description);
                List<Posts> posts = await PostsDal.GetPostsByCategory(category.Category_Id.ToString());

                foreach (Posts post in posts)
                {
                    string content = WebUtility.HtmlDecode(Regex.Replace(post.Post_Content, @"<[^>]*>", String.Empty));
                    string title = WebUtility.HtmlDecode(Regex.Replace(post.Post_Title, @"<[^>]*>", String.Empty));
                    group.Items.Add(new PostDataItem(category.Category_Id.ToString() + "-Item-" + post.ID,
                        title,
                        "",
                        post.Post_Image_Full_Url,
                        "",
                        content,
                        group));
                }
                this.AllGroups.Add(group);                
            }     
            #endregion       
        }
    }
}
