﻿using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace AspNet_JWT_Client.Infrastructure
{
    public class LocalStorageService : ILocalStorageService
    {
        private readonly IJSRuntime _jsRuntime;

        public LocalStorageService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task SetAsync<T>(string key, T item) where T : class
        {
            var data = JsonSerializer.Serialize(item);
            await _jsRuntime.InvokeVoidAsync("set", key, data);
        }

        public Task SetStringAsync(string key, string value)
        {
            _jsRuntime.InvokeAsync<string>("set", key, value);
            return Task.CompletedTask;
        }

        public async Task<T> GetAsync<T>(string key) where T : class
        {
            var data = await _jsRuntime.InvokeAsync<string>("get", key);
            if (string.IsNullOrEmpty(data))
            {
                return null!;
            }
            return JsonSerializer.Deserialize<T>(data)!;
        }

        public async Task<string> GetStringAsync(string key)
        {
            return await _jsRuntime.InvokeAsync<string>("get", key);
        }

        public Task RemoveAsync(string key)
        {
            _jsRuntime.InvokeAsync<string>("remove", key);
            return Task.CompletedTask;
        }
    }
}