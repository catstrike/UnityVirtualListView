using Controls;
using System;
using System.Collections.Generic;

namespace Examples.Simple
{
    public class StoreDataSource: IListViewDataSource<StoreItemData>
    {
        #region - Properties
        public int Count {
            get { return items.Count; }
        }
        #endregion

        #region - Events
        public event Action OnUpdate;
        #endregion

        #region - State
        List<StoreItemData> items;
        #endregion

        #region - Lifecycle
        public StoreDataSource()
        {
            items = new List<StoreItemData> {
                new StoreItemData {
                    Title = "Mazda 1",
                    Price = 18000
                },
                new StoreItemData {
                    Title = "Mazda 3",
                    Price = 26000
                },
                new StoreItemData {
                    Title = "Mazda 6",
                    Price = 32000
                },
                new StoreItemData {
                    Title = "BMW 3",
                    Price = 52000
                },
                new StoreItemData {
                    Title = "BMW 5",
                    Price = 61000
                },
                new StoreItemData {
                    Title = "BMW 7",
                    Price = 85000
                },
                new StoreItemData {
                    Title = "BMW X1",
                    Price = 51000
                },
                new StoreItemData {
                    Title = "BMW X3",
                    Price = 67000
                },
                new StoreItemData {
                    Title = "BMW X5",
                    Price = 83500
                },
                new StoreItemData {
                    Title = "Mercedes A-Class",
                    Price = 24600
                },
                new StoreItemData {
                    Title = "Mercedes B-Class",
                    Price = 27000
                },
                new StoreItemData {
                    Title = "Mercedes C-Class",
                    Price = 32000
                },
                new StoreItemData {
                    Title = "Mercedes CLA",
                    Price = 29700
                },
                new StoreItemData {
                    Title = "Mercedes CLS",
                    Price = 54800
                },
                new StoreItemData {
                    Title = "Mercedes E-Class",
                    Price = 43000
                },
                new StoreItemData {
                    Title = "Mercedes S-Class",
                    Price = 83000
                },
            };
        }
        #endregion

        #region - Public
        public StoreItemData GetItem(int index)
        {
            return items[index];
        }

        public void Update()
        {
            if (OnUpdate != null) {
                OnUpdate();
            }
        }
        #endregion
    }
}
