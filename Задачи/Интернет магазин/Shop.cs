using System;
using System.Collections;
using System.Collections.Generic;

public class ShopProgram
{
	class Good
    {
        public readonly string Name;

        public Good(string name)
        {
            Name = name;
        }
    }

    class Cart
    {
        private Dictionary<Good, int> _goods;
        private Shop _shop;

        public Cart(Shop shop)
        {
            _goods = new Dictionary<Good, int>();
            _shop = shop;
        }

        public IReadOnlyDictionary<Good, int> Goods => _goods;

        public void Add(Good good, int amount)
        {
            if (amount < 1)
                throw new Exception("Количество заказываемого товара не может быть меньше 1!");

            if (_shop.IsAvailable(good, amount))
            {
                if (_goods.ContainsKey[good])
                    _goods[good] += amount;
                else
                    _goods[good] = amount;
            }  
        }

        public void Remove(Good good, int amount)
        {
            if (amount < 1)
                throw new Exception("Количество удаляемого товара не может быть меньше 1!");

            if (_goods.ContainsKey[good])
            {
                if (_goods[good] > amount)
                    _goods[good] -= amount;
                else if (_goods[good] == amount)
                    _goods.Remove(good);
                else
                    throw new Exception("Нет такого количества товара в корзине!");
            }
            else
            {
                throw new Exception("Такого товара нет в корзине!");
            }
        }

        public Order Order()
        {
            return _shop.Order(this, _goods);
        }
    }

    class Order
    {
        public string Paylink { get; private set; }

        public Order(IReadOnlyDictionary<Good, int> availableGoods, IReadOnlyDictionary<Good, int> notAvailableGoods)
        {
            Paylink = "random-link.com";
            AvailableGoods = availableGoods;
            NotAvailableGoods = notAvailableGoods;
        }

        public IReadOnlyDictionary<Good, int> AvailableGoods { get; private set; }
        public IReadOnlyDictionary<Good, int> NotAvailableGoods { get; private set; }
    }

    class Shop
    {
        private Warehouse _warehouse;
        private List<Cart> _carts;

        public Shop(Warehouse warehouse)
        {
            _warehouse = warehouse;
        }

        public Cart Cart()
        {
            Cart cart = new Cart(this);
            _carts.Add(cart);

            return cart;
        }

        public bool IsAvailable(Good good, int amount)
        {
            return _warehouse.IsAvailable(good, amount);
        }

        public Order Order(Cart orderFromCart, IReadOnlyDictionary<Good, int> goods)
        {
            if (_carts.Contains(orderFromCart) == false)
                throw new Exception("Ошибка корзины!");

            Dictionary<Good, int> availableGoods = new Dictionary<Good, int>();
            Dictionary<Good, int> notAvailableGoods = new Dictionary<Good, int>();

            foreach (var goodPosition in goods)
            {
                int availableGoodAmount = _warehouse.AvailableAmount(goodPosition.Key);

                if (availableGoodAmount == 0)
                {
                    notAvailableGoods.Add(goodPosition.Key, goodPosition.Value);
                }
                else if (availableGoodAmount < goodPosition.Value)
                {
                    availableGoods.Add(goodPosition.Key, availableGoodAmount);
                    notAvailableGoods.Add(goodPosition.Key, goodPosition.Value - availableGoodAmount);
                }
                else
                {
                    availableGoods.Add(goodPosition.Key, goodPosition.Value);
                }
            }

            _warehouse.PullGoods(availableGoods);

            return new Order(availableGoods, notAvailableGoods);
        }
    }

    class Warehouse
    {
        private Dictionary<Good, int> _goods;

        public Warehouse()
        {
            _goods = new Dictionary<Good, int>();
        }

        public IReadOnlyDictionary<Good, int> Goods => _goods;

        public void Delieve(Good good, int amount)
        {
            if (amount < 1)
                throw new Exception("Количество доставляемого товара не может быть меньше 1!");

            if (_goods.ContainsKey(good))
                _goods[good] += amount;
            else
                _goods[good] = amount;
        }

        public void PullGoods(IReadOnlyDictionary<Good, int> goods)
        {
            foreach (var goodPosition in goods)
            {
                if (IsAvailable(goodPosition.Key, goodPosition.Value))
                {
                    _goods[good] -= amount;

                    if (_goods[good] == 0)
                        _goods.Remove(good);
                }
                else
                {
                    throw new Exception("Недостаточно товара на складе!");
                }
            }
        }

        public int AvailableAmount(Good good)
        {
            if (_goods.ContainsKey(good))
                return _goods[good];
            else
                return 0;
        }

        public bool IsAvailable(Good good, int amount)
        {
            if (amount < 1)
                throw new Exception("Количество заказываемого товара не может быть меньше 1!");

            return _goods.ContainsKey[good] == true && _goods[good] >= amount;
        }
    }
}
