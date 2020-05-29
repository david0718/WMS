﻿using Coldairarrow.Business.PB;
using Coldairarrow.Business.TD;
using Coldairarrow.Entity.TD;
using Coldairarrow.Util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Quartz.Util;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Api.Controllers.TD
{
    [Route("/TD/[controller]/[action]")]
    public class TD_OutStorageController : BaseApiController
    {
        #region DI

        public TD_OutStorageController(ITD_OutStorageBusiness tD_OutStorageBus, IServiceProvider provider, IOperator op)
        {
            _tD_OutStorageBus = tD_OutStorageBus;
            _provider = provider;
            _Op = op;
        }

        ITD_OutStorageBusiness _tD_OutStorageBus { get; }

        IServiceProvider _provider { get; }

        IOperator _Op { get; }

        #endregion

        #region 获取

        [HttpPost]
        public async Task<PageResult<TD_OutStorage>> GetDataList(TD_OutStoragePageInput input)
        {
            input.StorId = _Op.Property.DefaultStorageId;
            return await _tD_OutStorageBus.GetDataListAsync(input);
        }

        [HttpPost]
        public async Task<TD_OutStorage> GetTheData(IdInputDTO input)
        {
            return await _tD_OutStorageBus.GetTheDataAsync(input.id);
        }

        #endregion

        #region 提交

        [HttpPost]
        public async Task SaveData(TD_OutStorage data)
        {
            if (data.Id.IsNullOrEmpty())
            {
                InitEntity(data);
                data.StorageId = _Op.Property.DefaultStorageId;
                foreach (var item in data.OutStorDetails)
                {
                    InitEntity(item);
                    item.StorId = data.StorageId;
                    item.TotalAmt = item.Price * item.LocalNum;
                }
                await _tD_OutStorageBus.AddDataAsync(data);
            }
            else
            {
                await _tD_OutStorageBus.UpdateDataAsync(data);
            }
            //if (data.Id.IsNullOrEmpty())
            //{
            //    InitEntity(data);
            //    if (data.Code.IsNullOrWhiteSpace())
            //    {
            //        data.Code = await _provider.GetRequiredService<IPB_BarCodeTypeBusiness>().Generate("TD_OutStorage");
            //    }

            //    await _tD_OutStorageBus.AddDataAsync(data);
            //}
            //else
            //{
            //    await _tD_OutStorageBus.UpdateDataAsync(data);
            //}
        }

        [HttpPost]
        public async Task DeleteData(List<string> ids)
        {
            await _tD_OutStorageBus.DeleteDataAsync(ids);
        }

        #endregion
    }
}