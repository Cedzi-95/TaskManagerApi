import authApi from "./auth";  // Remove the curly braces for default import
export const createTask = async (taskDto) => {
  try {
    const response = await authApi.post("/task/create", taskDto);
    return response.data;
  } catch (error) {
    throw new Error(error.response?.data?.message || "Error creating task");
  }
};

export const getTasks = async () => {
  try {
    const response = await authApi.get("/task");
    // Map raw task data to task model instances if needed
    const tasks = response.data;
    const taskModels = tasks.map((task) => new Task(
      task.id,
      task.title,
      task.description,
      task.createdAt,
      task.deadline,
      task.isCompleted,
      task.isPriority,
      task.userId,
      task.user
    ));
    
    return {
      tasks: tasks, // raw data
      taskModels: taskModels // mapped task instances
    };
  } catch (error) {
    throw new Error(error.response?.data?.message || "Error fetching tasks");
  }
};

export const getTaskById = async (taskId) => {
  try {
    const response = await authApi.get(`/task/${taskId}`);
    return response.data;
  } catch (error) {
    throw new Error(error.response?.data?.message || "Error fetching task");
  }
};

export const updateTask = async (taskId, updateTaskDto) => {
  try {
    const response = await authApi.put(`/task/${taskId}`, updateTaskDto);
    return response.data;
  } catch (error) {
    throw new Error(error.response?.data?.message || "Error updating task");
  }
};

export const deleteTask = async (taskId) => {
  try {
    await authApi.delete(`/task/${taskId}`);
    return true;
  } catch (error) {
    throw new Error(error.response?.data?.message || "Error deleting task");
  }
};

export const completeTask = async (taskId) => {
  try {
    const response = await authApi.patch("/task/complete", { id: taskId });
    return response.data;
  } catch (error) {
    throw new Error(error.response?.data?.message || "Error completing task");
  }
};