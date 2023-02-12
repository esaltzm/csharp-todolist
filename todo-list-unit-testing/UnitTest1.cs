using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Threading.Tasks;
using Xunit;
using todo_list.Models;

namespace todo_list.Tests;

public class UnitTestModels
{
    private readonly string _connectionString = "Data Source=todo.db; Version=3;";

    [Fact]
    public async Task GetAllItems_ReturnsCorrectItems()
    {
        // Arrange
        var toDoList = new ToDoList();

        using (var connection = new SQLiteConnection(_connectionString))
        {
            await connection.OpenAsync();

            using (var command = new SQLiteCommand(connection))
            {
                command.CommandText = "INSERT INTO tasks (description) VALUES ('item 1'), ('item 2'), ('item 3')";
                await command.ExecuteNonQueryAsync();
            }
        }

        // Act
        var items = await toDoList.GetAllItems();

        // Assert
        Assert.Equal(3, items.Count);
        Assert.Equal("item 1", items[0].description);
        Assert.False(items[0].completed);
        Assert.Equal("item 2", items[1].description);
        Assert.True(items[1].completed);
        Assert.Equal("item 3", items[2].description);
        Assert.False(items[2].completed);
    }

    [Fact]
    public async Task AddItem_AddsItemToList_ReturnsToDoItem()
    {
        // Arrange
        var toDoList = new ToDoList();
        var description = "test item";

        // Act
        var result = await toDoList.AddItem(description);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(description, result.description);
        Assert.False(result.completed);
        Assert.IsType<int>(result.id);
        Assert.IsType<ToDoItem>(result);
    }
}
