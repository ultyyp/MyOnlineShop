using OnlineShop.Domain.Entities;

namespace OnlineShop.Domain.Test
{
    public class CartTests
    {
        [Fact]
        public void Item_is_added_to_cart()
        {
            //Arrange
            var cart = new Cart(Guid.NewGuid(), Guid.NewGuid());    
            var productId = Guid.NewGuid();

            //Act
            cart.AddItem(productId, 1);

            //Assert
            Assert.Single(cart.Items);
        }

        [Fact]
        public void Adding_existing_item_to_cart_increases_quantity()
        {
            //Arrange
            var cart = new Cart(Guid.NewGuid(), Guid.NewGuid());
            var productId = Guid.NewGuid();

            //Act
            cart.AddItem(productId, 1);
            cart.AddItem(productId, 1);

            //Assert
            Assert.Single(cart.Items);
            Assert.Equal(2d, cart.Items.First().Quantity);
        }

    }
}