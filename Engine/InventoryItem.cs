﻿using System.ComponentModel;

namespace Engine
{
	/// <summary>
	/// 背包中的物品，保存物品的信息和物品数量
	/// </summary>
	public class InventoryItem : INotifyPropertyChanged
	{
		private Item _details;
		private int _quantity;

		public Item Details
		{
			get { return _details; }
			set
			{
				_details = value;
				OnPropertyChanged("Details");
			}
		}

		public int Quantity
		{
			get { return _quantity; }
			set
			{
				_quantity = value;
				OnPropertyChanged("Quantity");
				OnPropertyChanged("Description");
			}
		}

		public int ItemID
		{
			get { return Details.ID; }
		}

		// 物品简介，获取物品名称（因为UI绑定时，不能通过Details.Name访问物品名）
		public string Description
		{
			get { return Quantity > 1 ? Details.NamePlural : Details.Name; }
		}

		public int Price
		{
			get { return Details.Price; }
		}

		/// <summary>
		/// 背包中的物品
		/// </summary>
		/// <param name="details">物品详情</param>
		/// <param name="quantity">物品数量</param>
		public InventoryItem(Item details, int quantity)
		{
			Details = details;
			Quantity = quantity;
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public void OnPropertyChanged(string name)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(name));
			}
		}
	}
}
