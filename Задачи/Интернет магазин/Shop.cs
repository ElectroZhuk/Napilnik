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
            if (name == null)
                throw new System.NullReferenceException("Имя не может быть null!");

            Name = name;
        }
    }

    class Cart
    {
        private Dictionary<Good, int> _goods;
        private Shop _shop;

        public Cart(Shop shop)
        {
            if (shop == null)
                throw new System.NullReferenceException("Магазин не может быть null!");

            _goods = new Dictionary<Good, int>();
            _shop = shop;
        }

        public IReadOnlyDictionary<Good, int> Goods => _goods;

        public void Add(Good good, int amount)
        {
            if (amount < 1)
                throw new System.ArgumentException("Количество добавляемого товара не может быть меньше 1!");

            if (_shop.IsAvailable(good, amount) == false)
            {
                throw new System.ArgumentException("Количество добавляемого товара не может быть больше количества товара на складе!");
            }
            else
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
                throw new System.ArgumentException("Количество удаляемого товара не может быть меньше 1!");

            if (_goods.ContainsKey[good] == false)
            {
                throw new System.ArgumentException("Такого товара нет в корзине!");
            }
            else
            {
                if (_goods[good] < amount)
                    throw new System.ArgumentException("Нет такого количества товара в корзине!");
                else if (_goods[good] > amount)
                    _goods[good] -= amount;
                else 
                    _goods.Remove(good);
            }
        }

        public Order Order()
        {
            return _shop.Order(this, _goods);
        }
    }

    class Order
    {
        public readonly string Paylink;

        public Order(IReadOnlyDictionary<Good, int> availableGoods, IReadOnlyDictionary<Good, int> notAvailableGoods)
        {
            if (availableGoods == null)
                throw new System.ArgumentException("Доступные товары не могут быть null!");

            if (notAvailableGoods == null)
                throw new System.ArgumentException("Недоступные товары не могут быть null!");

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
            if (warehouse == null)
                throw new System.NullReferenceException("Склад не может быть null!");

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
                throw new System.ArgumentException("Неизвестная корзина!");

            if (goods == null)
                throw new System.ArgumentException("Словарь товаров не может быть null!");

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
                throw new System.ArgumentException("Количество доставляемого товара не может быть меньше 1!");

            if (_goods.ContainsKey(good))
                _goods[good] += amount;
            else
                _goods[good] = amount;
        }

        public void PullGoods(IReadOnlyDictionary<Good, int> goods)
        {
            if (goods == null)
                throw new System.ArgumentException("Словарь товаров не может быть null!");

            foreach (var goodPosition in goods)
                if (IsAvailable(goodPosition.Key, goodPosition.Value) == false)
                    throw new System.ArgumentException("Недостаточно товара на складе!");
          
            foreach (var goodPosition in goods)
            {
                _goods[good] -= amount;

                if (_goods[good] == 0)
                    _goods.Remove(good);
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
                throw new System.ArgumentException("Количество заказываемого товара не может быть меньше 1!");

            return _goods.ContainsKey[good] == true && _goods[good] >= amount;
        }
    }
}
