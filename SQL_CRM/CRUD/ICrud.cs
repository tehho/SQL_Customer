﻿using System.Collections.Generic;

namespace SQL_CRM
{
    public interface ICrud<T>
    {
        void Create(T obj);
        List<T> Read(T obj);
        void Update(T obj);
        void Delete(T obj);
    }
}