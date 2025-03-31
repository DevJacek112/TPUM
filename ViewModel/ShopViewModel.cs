using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Model;

namespace ViewModel
{
    public class ShopViewModel : BaseViewModel
    {
        private readonly UIDataLoader _uiDataModel;

        public ObservableCollection<UIProductData> ObservableProducts { get; }

        public ShopViewModel()
        {
            _uiDataModel = new UIDataLoader();
            ObservableProducts = new ObservableCollection<UIProductData>(_uiDataModel._products);

            foreach (var product in ObservableProducts)
            {
                string basePath = AppDomain.CurrentDomain.BaseDirectory;
                product.ProductImage = Path.Combine(basePath, "Images", product.ProductImage);
                Console.WriteLine(product.ProductImage);
            }
        }
    }
}
