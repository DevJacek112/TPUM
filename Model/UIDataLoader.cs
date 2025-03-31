using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class UIDataLoader
    {
        public List<UIProductData> _products { get; }

        public UIDataLoader()
        {
            //temp solution, will be replaced with loading from json
            _products = new List<UIProductData>
            {
                new UIProductData { ProductName = "Piękna łódź", ProductDescription = "Piękna łódź na spokojne wody.", ProductImage = "lodka.jpeg" },
                new UIProductData { ProductName = "Śliczna łódź", ProductDescription = "Śliczna łódź na niespokojne wody.", ProductImage = "lodka.jpeg" }
            };
        }
    }
}
