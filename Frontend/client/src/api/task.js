import { API_BASE_URL } from "/api";
import { Task } from "../models/task";

export async function apiCreateTask (taskDto)
{
    const response = await fetch(API_BASE_URL + "/task/create", {
        method : "POST",
        headers : {
            "Content-Type" : "application/json"
        },
        body : JSON.stringify(taskDto),
    });

    if (!response.ok)
    {
        const error = await response.json();
        throw new Error("Error when creating task: " + error.message);
    }
    const task = response.json();
    return task;
}


export async function apiGetTasks() {
    const response = await fetch(API_BASE_URL + "/task") 
    if (!response.ok) {
        const error = await response.json();
        throw new Error ("Error fetching your tasks" + error.message);
    }    
    const tasks = await response.json();

    
}