﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BlogMVC.Data.FileManager
{
    public interface IFileManager
    {
        Task<string> SaveImage(IFormFile image);

        FileStream ImageStream(string image);
    }

}
