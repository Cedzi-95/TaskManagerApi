import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { createTask } from '../api/task';
import '../styles/createTaskPage.css';

function CreateTaskPage({ user }) {
  const navigate = useNavigate();
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);
  const [formData, setFormData] = useState({
    title: '',
    description: '',
    deadline: '',
    isPriority: false,
    isCompleted: false
  });

  const handleInputChange = (e) => {
    const { name, value, type, checked } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: type === 'checkbox' ? checked : value
    }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    
    if (!formData.title.trim()) {
      setError('Task title is required');
      return;
    }

    try {
      setLoading(true);
      setError(null);
      
      const taskData = {
        ...formData,
        title: formData.title.trim(),
        description: formData.description.trim(),
        deadline: formData.deadline || null,
        createdAt: new Date().toISOString()
      };

      await createTask(taskData);
      
      // Navigate back to homepage or tasks page
      navigate('/', { 
        state: { message: 'Task created successfully!' }
      });
      
    } catch (err) {
      setError(err.response?.data?.message || 'Failed to create task');
      console.error('Create task error:', err);
      setLoading(false);
    }
  };

  const handleCancel = () => {
    navigate(-1); // Go back to previous page
  };

  return (
    <div className="create-task-page">
      <div className="create-task-container">
        <div className="page-header">
          <h1>Create New Task</h1>
          <p>Add a new task to your list</p>
        </div>

        <form onSubmit={handleSubmit} className="create-task-form">
          {error && (
            <div className="error-alert">
              {error}
            </div>
          )}

          <div className="form-group">
            <label htmlFor="title">Task Title *</label>
            <input
              type="text"
              id="title"
              name="title"
              value={formData.title}
              onChange={handleInputChange}
              placeholder="Enter task title..."
              required
              maxLength={100}
              disabled={loading}
            />
          </div>

          <div className="form-group">
            <label htmlFor="description">Description</label>
            <textarea
              id="description"
              name="description"
              value={formData.description}
              onChange={handleInputChange}
              placeholder="Enter task description..."
              rows={4}
              maxLength={500}
              disabled={loading}
            />
          </div>

          <div className="form-group">
            <label htmlFor="deadline">Deadline</label>
            <input
              type="datetime-local"
              id="deadline"
              name="deadline"
              value={formData.deadline}
              onChange={handleInputChange}
              disabled={loading}
            />
          </div>

          <div className="form-checkboxes">
            <div className="checkbox-group">
              <input
                type="checkbox"
                id="isPriority"
                name="isPriority"
                checked={formData.isPriority}
                onChange={handleInputChange}
                disabled={loading}
              />
              <label htmlFor="isPriority">Mark as Priority</label>
            </div>

            <div className="checkbox-group">
              <input
                type="checkbox"
                id="isCompleted"
                name="isCompleted"
                checked={formData.isCompleted}
                onChange={handleInputChange}
                disabled={loading}
              />
              <label htmlFor="isCompleted">Mark as Completed</label>
            </div>
          </div>

          <div className="form-actions">
            <button
              type="button"
              onClick={handleCancel}
              className="btn-cancel"
              disabled={loading}
            >
              Cancel
            </button>
            <button
              type="submit"
              className="btn-create"
              disabled={loading}
            >
              {loading ? 'Creating...' : 'Create Task'}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}

export default CreateTaskPage;