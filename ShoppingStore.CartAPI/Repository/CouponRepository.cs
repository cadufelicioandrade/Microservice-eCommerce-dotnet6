﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ShoppingStore.CartAPI.Data.ValueObjects;
using ShoppingStore.CartAPI.Model.Context;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;

namespace ShoppingStore.CartAPI.Repository
{
    public class CouponRepository : ICouponRepository
    {
        private readonly HttpClient _httpClient;

        public CouponRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<CouponVO> GetCoupon(string couponCode, string token)
        {
            //"api/v1/coupon"
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync($"/api/v1/coupon/{couponCode}");
            var content = await response.Content.ReadAsStringAsync();

            if (response.StatusCode != HttpStatusCode.OK)
                return new CouponVO();

            return JsonSerializer.Deserialize<CouponVO>(content,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
    }
}
