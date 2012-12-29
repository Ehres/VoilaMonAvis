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
            // Autorise l'état de page enregistré à substituer l'élément initial à afficher
            if (pageState != null && pageState.ContainsKey("SelectedItem"))            
                navigationParameter = pageState["SelectedItem"];
            

            // TODO: créez un modèle de données approprié pour le domaine posant problème pour remplacer les exemples de données
            var item = PostDataSource.GetItem((String)navigationParameter);
            this.DefaultViewModel["Group"] = item.Group;
            this.DefaultViewModel["Items"] = item.Group.Items;
            
            System.Collections.ObjectModel.ObservableCollection<PostDataItem> items = new System.Collections.ObjectModel.ObservableCollection<PostDataItem>();
            items.Add(item);
            this.DefaultViewModel["Items"] = items;
            this.flipView.SelectedItem = item;            
        }
    }
}
