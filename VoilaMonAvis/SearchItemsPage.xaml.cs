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

// Pour en savoir plus sur le modèle d'élément Page Éléments, consultez la page http://go.microsoft.com/fwlink/?LinkId=234233

namespace VoilaMonAvis
{
    /// <summary>
    /// Page affichant une collection d'aperçus d'éléments. Dans le modèle Application fractionnée, cette page
    /// est utilisée pour afficher et sélectionner l'un des groupes disponibles.
    /// </summary>
    public sealed partial class SearchItemsPage : VoilaMonAvis.Common.LayoutAwarePage
    {
        public SearchItemsPage()
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
            // TODO: assignez une collection d'éléments pouvant être liée à this.DefaultViewModel["Items"]
        }
    }
}
