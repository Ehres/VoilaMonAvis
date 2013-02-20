using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Threading.Tasks;
using Windows.UI.ViewManagement;
using Windows.Graphics.Display;
using Windows.Storage.Pickers;
using Windows.Storage;
using VoilaMonAvis.DataModel;
using VoilaMonAvis.DataAccessLayer;
using System.Net;
using System.Text.RegularExpressions;

// Pour en savoir plus sur le modèle d'élément Page Détail de l'élément, consultez la page http://go.microsoft.com/fwlink/?LinkId=234232

namespace VoilaMonAvis
{
    /// <summary>
    /// Page affichant les détails d'un élément au sein d'un groupe, offrant la possibilité de
    /// consulter les autres éléments qui appartiennent au même groupe.
    /// </summary>
    public sealed partial class ItemDetailPage : VoilaMonAvis.Common.LayoutAwarePage
    {
        public ItemDetailPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Remplit la page à l'aide du contenu passé lors de la navigation. Tout état enregistré est également
        /// fourni lorsqu'une page est recréée à partir d'une session antérieure.
        /// </summary>
        /// <param name="navigationParameter">Valeur de paramètre passée à
        /// <see cref="Frame.Navigate(Type, Object)"/> lors de la requête initiale de cette page.
        /// </param>
        /// <param name="pageState">Dictionnaire d'état conservé par cette page durant une session
        /// antérieure. Null lors de la première visite de la page.</param>
        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
            GridContainerContent.AddHandler(PointerWheelChangedEvent, new PointerEventHandler(Bubble_PointerWheelChanged), true);

            // Autorise l'état de page enregistré à substituer l'élément initial à afficher
            if (pageState != null && pageState.ContainsKey("SelectedItem"))            
                navigationParameter = pageState["SelectedItem"];
            

            var item = PostDataSource.GetItem((String)navigationParameter);
            if (item == null)            
                GetItem((String)navigationParameter);            
            else
            {
                //this.DefaultViewModel["Group"] = item.Group;
                this.DefaultViewModel["Items"] = item.Group.Items;

                System.Collections.ObjectModel.ObservableCollection<PostDataItem> items = new System.Collections.ObjectModel.ObservableCollection<PostDataItem>();
                items.Add(item);
                this.DefaultViewModel["Items"] = items;
                this.flipView.SelectedItem = item;
            }           
        }

        private async void FindItem(string query)
        {
            List<PostDataItem> posts = await PostDataSource.Find(query);

            if (posts.Count() > 0)
            {
                this.DefaultViewModel["Items"] = posts;
                this.flipView.SelectedItem = posts.First();
            }
        }

        private async void GetItem(string id)
        {
            string group_id = id.Split('-').First();
            string post_id = id.Split('-').Last();
            var item = await PostsDal.GetPost(int.Parse(post_id));
            string content = WebUtility.HtmlDecode(Regex.Replace(item.Post_Content, @"<[^>]*>", String.Empty));

            PostDataItem pdi = new PostDataItem(item.ID.ToString(), item.Post_Title, item.Post_Image_Full_Url, content, PostDataSource.GetGroup(group_id), item.Post_Video_Url, 1);
            //this.DefaultViewModel["Group"] = item.Group;
            this.DefaultViewModel["Items"] = pdi.Group.Items;

            System.Collections.ObjectModel.ObservableCollection<PostDataItem> items = new System.Collections.ObjectModel.ObservableCollection<PostDataItem>();
            items.Add(pdi);
            this.DefaultViewModel["Items"] = items;
            this.flipView.SelectedItem = item;
        }

        void ButtonReturnHome_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(GroupedItemsPage), "AllGroups");
        }

        private void Bubble_PointerWheelChanged(object sender, PointerRoutedEventArgs e)
        { // Could walk up the tree to find the next SV or just have a reference like here: 
            //MyScrollViewer.ScrollToHorizontalOffset(MyScrollViewer.HorizontalOffset - e.GetCurrentPoint(null).Properties.MouseWheelDelta);
            
        }
    }
}
