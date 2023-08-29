using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Web_Api_NOTION;
using Web_Api_NOTION.Estructure_Json;
namespace Web_Api_NOTION.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotionController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        //Credenciales
        private string databaseId = "2eecf8516a124a69aa9607315c9d5617";
        private string NOTION_TOKEN = "secret_EQHJJsK52S3L6B2ad8tFJmtj8I5uPR3WglPPKFrg40b";
        private string notionVersion = "2022-06-28";

        public NotionController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://api.notion.com/v1/");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", NOTION_TOKEN);
            _httpClient.DefaultRequestHeaders.Add("Notion-Version", notionVersion);
        }
        //Solicitar datos de la bd de la api de notion
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetNotionData()
        {
            try
            {
                // Hacer la solicitud POST a la API de Notion para obtener los datos de la base de datos
                string url = $"databases/{databaseId}/query";

                var response = await _httpClient.PostAsync(url, null);

                // Verificar el estado de la respuesta
                if (response.IsSuccessStatusCode)
                {
                    // Procesar los datos de la respuesta según sea necesario
                    var responseBody = await response.Content.ReadAsStringAsync();

                    return Ok(responseBody);
                }
                else
                {
                    // Leer el cuerpo del error de la respuesta y devolverlo como resultado
                    var errorData = await response.Content.ReadAsStringAsync();
                    return BadRequest(errorData);
                }
            }
            catch (HttpRequestException ex)
            {
                return NotFound(ex.Message);
            }
        }
        //Crear una nueva fila en la BD de Notion
        [HttpPost("newRow")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(NotionDataDto))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> CreateNewRow([FromBody] NotionDataDto notionData)
        {
            try
            {
                // Crear el cuerpo de la nueva página
                // Crear una instancia de la clase EstructuraJson

                var estructuraJson = new EstructuraJson();
                var newPageContent = estructuraJson.CreateNewPageContent(notionData, databaseId);
                // Convertir el cuerpo a JSON
                var jsonBody = Newtonsoft.Json.JsonConvert.SerializeObject(newPageContent);
                var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                // Hacer la solicitud POST a la API de Notion para crear la nueva página
                var response = await _httpClient.PostAsync("pages", content);

                // Verificar el estado de la respuesta
                if (response.IsSuccessStatusCode)
                {
                    // Procesar los datos de la respuesta según sea necesario
                    var responseBody = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(responseBody);
                    // Aquí puedes deserializar la respuesta en un objeto si es necesario
                    // var createdPage = Newtonsoft.Json.JsonConvert.DeserializeObject<NotionDataDto>(responseBody);
                    return new OkObjectResult(responseBody);

                    //ver como devolver en que lugar fue creado
                    //return Created(response.Headers.Location, responseBody); // Devolver la ubicación de la nueva página creada
                }
                else
                {
                    // Leer el cuerpo del error de la respuesta y devolverlo como resultado
                    var errorData = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(errorData);
                    return BadRequest(errorData);
                }
            }
            catch (HttpRequestException exception)
            {
                return BadRequest(exception.Message);
            }
        }

        //modificar datos de una fila 
        [HttpPatch("updateRow/{pageId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(NotionDataDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status304NotModified)]
        public async Task<IActionResult> UpdateRow(string pageId, [FromBody] NotionDataDto notionData)
        {
            try
            {
                // Crear el cuerpo de la actualización
                var estructuraJson = new EstructuraJson();
                var updatePageContent = estructuraJson.CreateUpdatePageContent(notionData, databaseId);
                Console.WriteLine(updatePageContent);
                Console.WriteLine(notionData.fecha_de_realizacion);

                // Convertir el cuerpo a JSON
                var jsonBody = Newtonsoft.Json.JsonConvert.SerializeObject(updatePageContent);
                var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
                Console.WriteLine(content);
                // Construir la URL para la actualización usando el pageId del parámetro
                string url = $"pages/{pageId}";

                // Hacer la solicitud PATCH a la API de Notion para actualizar la página
                var response = await _httpClient.PatchAsync(url, content);
                Console.WriteLine(response.StatusCode);
                // Verificar el estado de la respuesta
                if (response.IsSuccessStatusCode)
                {
                    // Procesar los datos de la respuesta según sea necesario
                    var responseBody = await response.Content.ReadAsStringAsync();

                    // Aquí puedes deserializar la respuesta en un objeto si es necesario
                    // var updatedPage = Newtonsoft.Json.JsonConvert.DeserializeObject<NotionDataDto>(responseBody);
                    Console.WriteLine(responseBody);
                    return Ok(responseBody);
                }
                else
                {
                    // Leer el cuerpo del error de la respuesta y devolverlo como resultado
                    var errorData = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error: {errorData}");   
                    return BadRequest(errorData);
                }
            }
            catch (HttpRequestException exception)
            {
                Console.WriteLine(exception.Message );
                return BadRequest(exception.Message);
            }
        }



    }
}
