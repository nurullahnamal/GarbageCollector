using System;
using System.Collections.Generic;

namespace GarbageCollector
{
	class Program
	{
		static void Main(string[] args)
		{
			double totalOrderPrice = CreateShortLivedOrders();

			GC.Collect();
			GC.WaitForPendingFinalizers();

			Console.WriteLine(new string('-', 25));

			CreatePersistentCustomers();
			GC.Collect();
			GC.WaitForPendingFinalizers();
		}

		static double CreateShortLivedOrders()
		{
			double totalPrice = 0;
			for (int i = 0; i < 4; i++)
			{
				Order order = new Order($"Order {i + 1} ", 100 + i * 50);
				totalPrice += order.Price;
			}
			return totalPrice; // Added return statement
		}

		static void CreatePersistentCustomers()
		{
			Customer customer = new Customer("John Doe");
			customer.AddOrder(new Order("123", 250));
			customer.AddOrder(new Order("456", 300));
		}

		class Customer
		{
			public string Name { get; private set; }

			private List<Order> orders = new List<Order>();
			public Customer(string name)
			{
				Name = name;
			}

			public void AddOrder(Order order)
			{
				orders.Add(order);
			}

			~Customer()
			{
				Console.WriteLine($"{Name} müşterisi için finalize çalıştı");
			}
		}

		class Order
		{
			public string Id { get; private set; }
			public double Price { get; private set; }

			public Order(string id, double price)
			{
				Id = id;
				Price = price;
			}

			~Order()
			{
				Console.WriteLine($"{Id} id'li sipariş için finalizer çalıştı");
			}
		}
	}
}
