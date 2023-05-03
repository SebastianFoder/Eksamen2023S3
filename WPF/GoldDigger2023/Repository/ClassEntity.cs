using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class ClassEntity : ClassNotify, IPersistable
    {
        private int _Id;


        public ClassEntity()
        {
            Id = 0;
        }

        public ClassEntity(int inId)
        {
            Id = inId;
        }

        public ClassEntity(ClassEntity inEntity)
        {
            Id = inEntity.Id;
        }

        public int Id
        {
            get { return _Id; }
            private set
            {
                if (_Id != value)
                {
                    _Id = value;
                }
                Notify("Id");
            }
        }
    }
}