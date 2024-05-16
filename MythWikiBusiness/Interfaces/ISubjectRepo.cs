using System;
using MythWikiBusiness.Models;
namespace MythWikiBusiness.IRepository
{
	public interface ISubjectRepo
	{
		public IQueryable<Subject> GetSubjects();
	}
}

