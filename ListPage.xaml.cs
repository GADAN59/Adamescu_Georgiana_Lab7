using Adamescu_Georgiana_Lab7.Models;

namespace Adamescu_Georgiana_Lab7;

public partial class ListPage : ContentPage
{
	public ListPage()
	{
		InitializeComponent();
	}

	async void OnSaveButtonClicked(object sender, EventArgs e)
	{
		var slist = (ShopList)BindingContext;
		slist.Date = DateTime.UtcNow;
		await App.Database.SaveShopListAsync(slist);
		await Navigation.PopAsync();
	}

	async void OnDeleteButtonClicked(object sender, EventArgs e)
	{
		var slist = (ShopList)BindingContext;
		await App.Database.DeleteShopListAsync(slist);
		await Navigation.PopAsync();
	}

    async void OnChooseButtonClicked(object sender, EventArgs e) 
	{ 
		await Navigation.PushAsync(new ProductPage((ShopList)this.BindingContext) 
		{ 
			BindingContext = new Product() 
		}); 
	}

    async void OnDeleteItemButtonClicked(object sender, EventArgs e)
    {
        var shopList = (ShopList)BindingContext;

		if(listView.SelectedItem!=null)
		{
            var selectedProduct = listView.SelectedItem as Product;
			var allProducts = await App.Database.GetAllProducts();
			var productsToDelete = allProducts.Find(listProduct => listProduct.ProductID == selectedProduct.ID && listProduct.ShopListID == shopList.ID);

			if(productsToDelete != null)
			{
				await App.Database.DeleteListProductAsync(productsToDelete);
				await Navigation.PopAsync();
			}

        }



        /*var selectedProduct = (Product)listView.SelectedItem;

        if (selectedProduct != null)
        {
            var shopList = (ShopList)BindingContext;

            // Assuming you have a method in your database to delete a product
            await App.Database.DeleteProductAsync(selectedProduct);

            // Refresh the ListView after deletion
            listView.ItemsSource = await App.Database.GetListProductsAsync(shopList.ID);
        }

        // Navigate back to the previous page
        await Navigation.PopAsync();*/
    }

    protected override async void OnAppearing() 
	{ 
		base.OnAppearing(); 
		var shopl = (ShopList)BindingContext; 
		listView.ItemsSource = await App.Database.GetListProductsAsync(shopl.ID); 
	}
}