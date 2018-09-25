﻿/* https://github.com/jwaliszko/ExpressiveAnnotations
 * Copyright (c) 2014 Jarosław Waliszko
 * Licensed MIT: http://opensource.org/licenses/MIT */

using System;
using System.Collections;
using Microsoft.AspNetCore.Http;

namespace ExpressiveAnnotations.NetCore.Caching
{
    /// <summary>
    ///     Persists arbitrary key-value pairs for the lifespan of the current HTTP request.
    /// </summary>
    public static class RequestStorage
    {
        private static IHttpContextAccessor _accessor;
        public static void Configure(IHttpContextAccessor httpContextAccessor)
        {
            _accessor = httpContextAccessor;
        }

        public static HttpContext HttpContext => _accessor.HttpContext;

        private static IDictionary Items
        {
            get
            {
                if (HttpContext == null)
                    throw new ApplicationException("HttpContext not available.");
                return HttpContext.Items as IDictionary;  // location that could be used throughtout the entire HTTP request lifetime
            }                                             // (contrary to a session, this one exists only within the period of a single request).
        }

        public static T Get<T>(string key)
        {
            return Items[key] == null
                ? default(T)
                : (T) Items[key];
        }

        public static void Set<T>(string key, T value)
        {
            Items[key] = value;
        }

        public static void Remove(string key)
        {
            Items.Remove(key);
        }
    }
}
