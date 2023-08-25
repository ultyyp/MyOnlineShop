using OnlineShop.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Domain.Entities
{
	public class PageCounter : IEntity
	{
		private PageCounter() { }

		public PageCounter(string domain, int count)
		{
			Id = Guid.NewGuid();
			this.Domain = domain;
			Count = count;
		}

		public Guid Id { get; init; }
		public string Domain { get; set; }
		public int Count { get; set; }
	}
}
