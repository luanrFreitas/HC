using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using HustleCastle.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

using Data = Google.Apis.Sheets.v4.Data;

namespace HustleCastle.Data
{
    public class SheetsDatabase 
    {
        static private string[] Scopes = { SheetsService.Scope.Spreadsheets };
        static string ApplicationName;
        static string SpreadsheetId;
        static SheetsService Service;
        GoogleCredential Credential;

        public SheetsDatabase(string applicationName, string spreadsheetId, FileStream stream)
        {
            ApplicationName = applicationName;
            SpreadsheetId = spreadsheetId;
            Credential = GoogleCredential.FromStream(stream).CreateScoped(Scopes);
            Service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = Credential,
                ApplicationName = ApplicationName,
            });
        }

        public List<T> GetAll<T>() where T : new()
        {
            PropertyInfo[] properties = typeof(T).GetProperties();
            var sheetRange = $"{typeof(T).Name}!A2:{(Column)properties.Length}";
            SpreadsheetsResource.ValuesResource.GetRequest request = Service.Spreadsheets.Values.Get(SpreadsheetId, sheetRange);

            var response = request.Execute();
            IList<IList<object>> sheetValues = response.Values;
            List<T> responseList = new List<T>();
            if (sheetValues != null && sheetValues.Count > 0)
            {
                foreach (var row in sheetValues)
                {
                    T instance = new T();
                    properties[0].SetValue(instance, sheetValues.IndexOf(row) + 2);
                    for (int i = 0; i < properties.Length - 1; i++)
                    {
                        properties[i + 1].SetValue(instance, row[i]);
                    }
                    responseList.Add(instance);
                }
            }
            else
            {
                Console.WriteLine("No data found.");
            }
            return responseList;
            // return (List<T>)Convert.ChangeType(respondeList, typeof(List<T>));
        }

        public T GetByID<T>(int id) where T : new()
        {
            PropertyInfo[] properties = typeof(T).GetProperties();
            var sheetRange = $"{typeof(T).Name}!A{id}:{(Column)properties.Length}{id}";

            SpreadsheetsResource.ValuesResource.GetRequest request = Service.Spreadsheets.Values.Get(SpreadsheetId, sheetRange);

            var response = request.Execute();
            IList<IList<object>> sheetValues = response.Values;
            T instance = default(T);
            if (sheetValues != null && sheetValues.Count > 0)
            {
                foreach (var row in sheetValues)
                {
                    instance = new T();
                    properties[0].SetValue(instance, id);

                    for (int i = 0; i < properties.Length - 1; i++)
                    {
                        properties[i + 1].SetValue(instance, row[i]);
                    }
                }
            }
            else
            {
                Console.WriteLine("No data found.");
            }
            return instance;
            // return (List<T>)Convert.ChangeType(respondeList, typeof(List<T>));
        }

        public void Add<T>(T source) where T : new()
        {
            PropertyInfo[] properties = typeof(T).GetProperties();
            //  var range = $"{nameof(T)}!A:F";
            var sheetRange = $"{typeof(T).Name}!A:{(Column)properties.Length - 1}";

            var valueRange = new ValueRange();
            var addList = new List<object>();
            for (int i = 1; i < properties.Length; i++)
            {
                addList.Add(source.GetType().GetProperty(properties[i].Name).GetValue(source));
            }
            valueRange.Values = new List<IList<object>> { addList };

            var appendRequest = Service.Spreadsheets.Values.Append(valueRange, SpreadsheetId, sheetRange);
            appendRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.USERENTERED;
            var appendReponse = appendRequest.Execute();
        }

        public void Update<T>(T source)
        {
            PropertyInfo[] properties = typeof(T).GetProperties();

            var range = $"{typeof(T).Name}!A{source.GetType().GetProperty("ID").GetValue(source)}";
            var valueRange = new ValueRange();

            var updateList = new List<object>();

            for (int i = 1; i < properties.Length; i++)
            {
                updateList.Add(source.GetType().GetProperty(properties[i].Name).GetValue(source));
            }

            valueRange.Values = new List<IList<object>> { updateList };

            var updateRequest = Service.Spreadsheets.Values.Update(valueRange, SpreadsheetId, range);
            updateRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED;
            var appendReponse = updateRequest.Execute();

        }

        public void Delete<T>(int id)
        {
            List<Google.Apis.Sheets.v4.Data.DataFilter> dataFilters = new List<Google.Apis.Sheets.v4.Data.DataFilter>();
            bool includeGridData = false;

            Google.Apis.Sheets.v4.Data.GetSpreadsheetByDataFilterRequest requestBody = new Google.Apis.Sheets.v4.Data.GetSpreadsheetByDataFilterRequest();
            requestBody.DataFilters = dataFilters;
            requestBody.IncludeGridData = includeGridData;

            SpreadsheetsResource.GetByDataFilterRequest request = Service.Spreadsheets.GetByDataFilter(requestBody, SpreadsheetId);
            Google.Apis.Sheets.v4.Data.Spreadsheet response = request.Execute();

            var sheetID = response.Sheets.Where(x => x.Properties.Title == typeof(T).Name).FirstOrDefault().Properties.SheetId;
            //DELETE THIS ROW
            Request RequestBody = new Request()
            {
                DeleteDimension = new DeleteDimensionRequest()
                {
                    Range = new DimensionRange()
                    {
                        SheetId = sheetID,
                        Dimension = "ROWS",
                        StartIndex = id,
                        EndIndex = id + 1
                    }
                }
            };

            List<Request> RequestContainer = new List<Request>();
            RequestContainer.Add(RequestBody);

            BatchUpdateSpreadsheetRequest DeleteRequest = new BatchUpdateSpreadsheetRequest();
            DeleteRequest.Requests = RequestContainer;

            SpreadsheetsResource.BatchUpdateRequest Deletion = new SpreadsheetsResource.BatchUpdateRequest(Service, DeleteRequest, SpreadsheetId);
            Deletion.Execute();


            //var range = $"{typeof(T).Name}!A{id}:F{id}";
            //var requestBody = new ClearValuesRequest();

            //var deleteRequest = Service.Spreadsheets.Values.Clear(requestBody, SpreadsheetId, range);
            //var deleteReponse = deleteRequest.Execute();
        }

    }
}
