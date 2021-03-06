﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VoilaMonAvis.DataModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Pour en savoir plus sur le modèle d'élément Page Éléments groupés, consultez la page http://go.microsoft.com/fwlink/?LinkId=234231

namespace VoilaMonAvis
{
    /// <summary>
    /// Page affichant une collection groupée d'éléments.
    /// </summary>
    public sealed partial class GroupedItemsPage : VoilaMonAvis.Common.LayoutAwarePage
    {
        public GroupedItemsPage()
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

            var postDataGroups = PostDataSource.GetGroups((String)navigationParameter);
            this.DefaultViewModel["Groups"] = postDataGroups;
        }

        void Header_Click(object sender, RoutedEventArgs e)
        {
            // Déterminez le groupe représenté par l'instance Button
            var group = (sender as FrameworkElement).DataContext;

            // Accédez à la page de destination souhaitée, puis configurez la nouvelle page
            // en transmettant les informations requises en tant que paramètre de navigation.
            this.Frame.Navigate(typeof(GroupDetailPage), ((PostDataGroup)group).UniqueId);
        }

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

        private void Grid_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            //customAnimation.Begin();
        }

        private void Bubble_PointerWheelChanged(object sender, PointerRoutedEventArgs e)
        { // Could walk up the tree to find the next SV or just have a reference like here: 
            MyScrollViewer.ScrollToHorizontalOffset(MyScrollViewer.HorizontalOffset - e.GetCurrentPoint(null).Properties.MouseWheelDelta);
        }
    }
}
