﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
	public class Player : LivingCreature
	{
		// 金币
		public int Gold { get; set; }
		// 经验值
		public int ExperiencePoints { get; set; }
		// 等级
		public int Level
		{
			// 每一次获取Level的值时，都会计算
			// 因为时整数型，会向下取整，所以需要+1
			get { return ((ExperiencePoints / 100) + 1); }
			// 我们不需要手动修改等级，所以把set删掉了。
		}
		// 物品栏
		public List<InventoryItem> Inventory { get; set; }
		// 当前任务
		public List<PlayerQuest> Quests { get; set; }
		// 当前位置
		public Location CurrentLocation { get; set; }

		/// <summary>
		/// 玩家生命值
		/// </summary>
		/// <param name="currentHitPoints">当前生命值</param>
		/// <param name="maximumHitPoints">最大生命值</param>
		/// <param name="gold">当前拥有金币</param>
		/// <param name="experiencePoints">经验值</param>
		/// <param name="level">等级</param>
		public Player(int currentHitPoints, int maximumHitPoints, int gold, int experiencePoints)
			: base(currentHitPoints, maximumHitPoints)
		{
			Gold = gold;
			ExperiencePoints = experiencePoints;

			Inventory = new List<InventoryItem>();
			Quests = new List<PlayerQuest>();
		}

		/// <summary>
		/// 检查玩家是否拥有进入这个场景所需要的关键道具
		/// </summary>
		/// <param name="location">所需要进入的场景</param>
		/// <returns>如果拥有钥匙道具或者不需要钥匙道具，则返回true，否则返回false</returns>
		public bool HasRequiredItemToEnterThisLocation(Location location)
		{
			if (location.ItemRequiredToEnter == null)
			{
				// 如果不需要关键道具进入这个场景，则直接返回true
				return true;
			}
			// 检查玩家物品栏里是否有所需要的关键道具，使用LINQ写法遍历玩家物品栏
			// Exists方法用于检查Inventory列表里有没有该道具，则返回true，否则返回false
			return Inventory.Exists(inventoryItem => inventoryItem.Details.ID == location.ItemRequiredToEnter.ID);
		}

		/// <summary>
		/// 检查玩家是否拥有这个任务
		/// </summary>
		/// <param name="quest">需要检查的任务</param>
		/// <returns>如果有任务则返回true</returns>
		public bool HasThisQuest(Quest quest)
		{
			return Quests.Exists(playerQuest => playerQuest.Details.ID == quest.ID);
		}

		/// <summary>
		/// 检查玩家是否已经完成该任务
		/// </summary>
		/// <param name="quest">需要检查的任务</param>
		/// <returns>返回任务完成情况，如果没有接过这个任务，则返回false</returns>
		public bool CompletedThisQuest(Quest quest)
		{
			foreach (PlayerQuest playerQuest in Quests)
			{
				if (playerQuest.Details.ID == quest.ID)
				{
					return playerQuest.IsCompleted;
				}
			}
			return false;
		}

		/// <summary>
		/// 检查玩家是否拥有完成任务的所有条件道具
		/// </summary>
		/// <param name="quest">需要检查的任务</param>
		/// <returns>玩家拥有所有条件道具，并且所有道具数量足够则返回true</returns>
		public bool HasAllQuestCompletionItems(Quest quest)
		{
			foreach (QuestCompletionItem qci in quest.QuestCompletionItems)
			{
				// 检查玩家物品栏里是否有对应的道具，如果有，检查他们的数量是否达到要求
				if (!Inventory.Exists(ii => ii.Details.ID == qci.Details.ID && ii.Quantity >= qci.Quantity))
				{
					// 如果没有达到其中一项要求，则直接返回false，不用继续检查后面的条件。
					return false;
				}
			}
			// 如果玩家拥有所有条件道具，并且所有道具数量足够，可以完成任务
			return true;
		}

		/// <summary>
		/// 删除玩家任务栏里的任务道具
		/// </summary>
		/// <param name="quest">执行的任务</param>
		public void RemoveQuestCompletetionItems(Quest quest)
		{
			foreach (QuestCompletionItem qci in quest.QuestCompletionItems)
			{
				// SingleOrDefault 函数用来检查列表里唯一一个匹配的物品
				InventoryItem item = Inventory.SingleOrDefault(ii => ii.Details.ID == qci.Details.ID);
				if (item != null)
				{
					// 减去任务道具
					item.Quantity -= qci.Quantity;
				}
			}
		}

		/// <summary>
		/// 添加物品到物品栏
		/// </summary>
		/// <param name="itemToAdd">需要添加的物品</param>
		public void AddItemToInventory(Item itemToAdd)
		{
			InventoryItem item = Inventory.SingleOrDefault(ii => ii.Details.ID == itemToAdd.ID);
			if (item == null)
			{
				// 如果物品栏里没有这个道具，则添加新的道具，数量为1
				Inventory.Add(new InventoryItem(itemToAdd, 1));
			}
			else
			{
				// 这个物品已在物品栏，所以直接增加物品数量就行
				item.Quantity++;
				return; // 添加完了之后，结束函数
			}
		}

		/// <summary>
		/// 将任务标记完成
		/// </summary>
		/// <param name="quest"></param>
		public void MarkQuestCompleted(Quest quest)
		{
			// 在玩家任务列表里找到该任务
			PlayerQuest playerQuest = Quests.SingleOrDefault(pq => pq.Details.ID == quest.ID);
			if (playerQuest != null)
			{
				// 把它标记任务完成
				playerQuest.IsCompleted = true;
			}
		}
	}
}
