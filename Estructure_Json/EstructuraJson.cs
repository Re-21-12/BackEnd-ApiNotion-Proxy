using System.ComponentModel;

namespace Web_Api_NOTION.Estructure_Json
{
    public class EstructuraJson
    {
        public object CreateNewPageContent(NotionDataDto notionData, string databaseId)
        {
         
                var newPageContent = new
                {
                    parent = new
                    {
                        database_id = databaseId
                    },
                    properties = new
                    {
                        titulo = new
                        {
                            title = new[]
                            {
                                new
                                {
                                    text = new
                                    {
                                        content = notionData.titulo
                                    }
                                }
                            }
                        },
                        descripcion = new
                        {
                            rich_text = new[]
                            {
                                new
                                {
                                    text = new
                                    {
                                        content = notionData.descripcion
                                    }
                                }
                            }
                        },
                        autor = new
                        {
                            rich_text = new[]
                            {
                                new
                                {
                                    text = new
                                    {
                                        content = notionData.autor
                                    }
                                }
                            }
                        },
                        fecha_de_realizacion = new
                        {
                            date = new
                            {
                                start = notionData.fecha_de_realizacion.ToString(),
                            }
                        },
                        hecha = new
                        {
                            checkbox = notionData.hecha
                        }
                    }
                };

                return newPageContent;
            
        
        }

        public object CreateUpdatePageContent(NotionDataDto notionData, string databaseId)
        {
            {
                var updatePageContent = new
                {
                    parent = new
                    {
                        database_id = databaseId
                    },
                    properties = new
                    {
                        titulo = new
                        {
                            title = new[]
                            {
                                new
                                {
                                    text = new
                                    {
                                        content = notionData.titulo
                                    }
                                }
                            }
                        },
                        descripcion = new
                        {
                            rich_text = new[]
                            {
                                new
                                {
                                    text = new
                                    {
                                        content = notionData.descripcion
                                    }
                                }
                            }
                        },
                        autor = new
                        {
                            rich_text = new[]
                            {
                                new
                                {
                                    text = new
                                    {
                                        content = notionData.autor
                                    }
                                }
                            }
                        },
                        fecha_de_realizacion = new
                        {
                            date = new
                            {
                                start = notionData.fecha_de_realizacion.ToString()
                            }
                        },
                        hecha = new
                        {
                            checkbox = notionData.hecha
                        }
                    }
                };

                return updatePageContent;
            }
     
        }
    }
}
