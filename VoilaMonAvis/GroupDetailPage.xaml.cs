﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using VoilaMonAvis.DataAccessLayer;
using VoilaMonAvis.DataModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Pour en savoir plus sur le modèle d'élément Page Détail du groupe, consultez la page http://go.microsoft.com/fwlink/?LinkId=234229

namespace VoilaMonAvis
{
    /// <summary>
    /// Page affichant une vue d'ensemble d'un groupe, ainsi qu'un aperçu des éléments
    /// qu'il contient.
    /// </summary>
    public sealed partial class GroupDetailPage : VoilaMonAvis.Common.LayoutAwarePage
    {
        public string idGroup = "";
        public PostDataGroup group = null;
        public GroupDetailPage()
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

            progressBarAllItemLoaded.Visibility = Visibility.Visible;
            // TODO: créez un modèle de données approprié pour le domaine posant problème pour remplacer les exemples de données
            var groupSelected = PostDataSource.GetGroup((String)navigationParameter);
            group = groupSelected;
            this.DefaultViewModel["Group"] = group;
            this.DefaultViewModel["Items"] = group.Items;
            idGroup = group.UniqueId;

            pageTitle.Text += " " + group.Title;

            LoadMoreItemGroup((String)navigationParameter);
        }

        private async void LoadMoreItemGroup(string uniqueId)
        {
            bool itemsLoaded = await PostDataSource.GetMoreItemGroup(uniqueId);
            if (itemsLoaded)
                progressBarAllItemLoaded.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Invoqué lorsqu'un utilisateur clique sur un élément.
        /// </summary>
        /// <param name="sender">GridView (ou ListView lorsque l'état d'affichage de l'application est Snapped)
        /// affichant l'élément sur lequel l'utilisateur a cliqué.</param>
        /// <param name="e">Données d'événement décrivant l'élément sur lequel l'utilisateur a cliqué.</param>
        void ItemView_ItemClick(object sender, ItemClickEventArgs e)
        {
            // Accédez à la page de destination souhaitée, puis configurez la nouvelle page
            // en transmettant les informations requises en tant que paramètre de navigation.
            var itemId = ((PostDataItem)e.ClickedItem).UniqueId;
            this.Frame.Navigate(typeof(ItemDetailPage), itemId);
        }



        void ButtonReturnHome_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(GroupedItemsPage), "AllGroups");
        }

        private void Bubble_PointerWheelChanged(object sender, PointerRoutedEventArgs e)
        { // Could walk up the tree to find the next SV or just have a reference like here: 
            MyScrollViewer.ScrollToHorizontalOffset(MyScrollViewer.HorizontalOffset - e.GetCurrentPoint(null).Properties.MouseWheelDelta);
        }
    }
}
