﻿using System.ComponentModel;

namespace Engine
{
	/// <summary>
	/// 背包中的物品，保存物品的信息和物品数量
	/// </summary>
	public class InventoryItem : INotifyPropertyChanged
	{
		// 物品详情
		private Item _details;
		public Item Details
		{
			get { return _details; }
			set
			{
				_details = value;
				OnPropertyChanged("Details");
			}
		}
		// 物品数量
		private int _quantity;
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
		// 物品简介，获取物品名称（因为UI绑定时，不能通过Details.Name访问物品名）
		public string Description
		{
			get { return Quantity > 1 ? Details.NamePlural : Details.Name; }
		}
		// 【只读】获取物品ID
		public int ItemID
		{
			get { return Details.ID; }
		}
		// 【只读】获取物品价格
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
