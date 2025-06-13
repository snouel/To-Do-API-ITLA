//Este archivo fue creado con fines de demostración y no debe ser utilizado en producción ya que su función es únicamente para mostrar cómo se puede establecer una conexión con SignalR desde el cliente y recibir notificaciones de eventos en tiempo real.

//NOTA: El cliente fue ejecutado y testeado en un proyecto aparte.

using Microsoft.AspNetCore.SignalR.Client;

var connection = new HubConnectionBuilder().WithUrl("https://localhost:7101/taskHub")
    .WithAutomaticReconnect().Build();

connection.On<object>("TaskCreated", newTask =>
{
    Console.WriteLine($"A new task has been created: {newTask}");
}
);

try
{
    await connection.StartAsync();
    Console.WriteLine("Connected to the Task Hub.");
}
catch (Exception ex)
{
    Console.WriteLine($"Error connecting to the Task Hub: {ex.Message}");
}

Console.ReadLine();
