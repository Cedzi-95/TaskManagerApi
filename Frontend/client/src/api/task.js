import authApi from "./auth";

export const createTask = async (taskDto) => {
    try {
      console.log('📝 Creating task:', JSON.stringify(taskDto, null, 2));
      
      // Ensure the token exists before making the request
      const token = localStorage.getItem('authToken');
      if (!token) {
        throw new Error('No authentication token found');
      }
  
      const response = await authApi.post("/task/create", taskDto);
      console.log('✅ Task created successfully', response.data);
      return response.data;
    } catch (error) {
      console.error('❌ Create task error:', {
        message: error.message,
        response: error.response?.data,
        status: error.response?.status,
        config: {
          url: error.config?.url,
          method: error.config?.method,
          data: error.config?.data
        }
      });
      
      if (error.response?.status === 401) {
        // Trigger token refresh if available
        if (localStorage.getItem('refreshToken')) {
          try {
            await authApi.post('/refresh', {
              refreshToken: localStorage.getItem('refreshToken')
            });
            // Retry the request after refresh
            return createTask(taskDto);
          } catch (refreshError) {
            console.error('Token refresh failed:', refreshError);
            throw new Error('Session expired. Please login again.');
          }
        }
        throw new Error('Session expired. Please login again.');
      }
      
      throw new Error(
        error.response?.data?.message || 
        error.message || 
        'Failed to create task'
      );
    }
  };
  
export const getTasks = async () => {
  try {
    console.log('📋 Fetching tasks...');
    const response = await authApi.get("/task");
    console.log('✅ Tasks fetched successfully:', response.data?.length || 0, 'tasks');
    
    // Return the data in the format ViewTasksPage expects
    return {
      data: response.data || [] // Ensure it's always an array
    };
  } catch (error) {
    console.error('❌ Get tasks error:', error);
    
    // Provide more specific error messages
    if (error.response?.status === 401) {
      throw new Error("Authentication required. Please log in again.");
    } else if (error.response?.status === 403) {
      throw new Error("Access denied. You don't have permission to view tasks.");
    } else if (error.code === 'ECONNREFUSED') {
      throw new Error("Cannot connect to server. Please check your connection.");
    }
    
    throw new Error(error.response?.data?.message || "Error fetching tasks");
  }
};

export const getTaskById = async (taskId) => {
  try {
    const response = await authApi.get(`/task/${taskId}`);
    return response.data;
  } catch (error) {
    console.error('Get task by ID error:', error);
    throw new Error(error.response?.data?.message || "Error fetching task");
  }
};

export const updateTask = async (taskId, updateTaskDto) => {
  try {
    const response = await authApi.put(`/task/${taskId}`, updateTaskDto);
    return response.data;
  } catch (error) {
    console.error('Update task error:', error);
    throw new Error(error.response?.data?.message || "Error updating task");
  }
};

export const deleteTask = async (taskId) => {
  try {
    await authApi.delete(`/task/${taskId}`);
    return true;
  } catch (error) {
    console.error('Delete task error:', error);
    throw new Error(error.response?.data?.message || "Error deleting task");
  }
};

export const completeTask = async (taskId) => {
  try {
    const response = await authApi.patch("/task/complete", { id: taskId });
    return response.data;
  } catch (error) {
    console.error('Complete task error:', error);
    throw new Error(error.response?.data?.message || "Error completing task");
  }
};