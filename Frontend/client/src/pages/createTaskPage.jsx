import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { createTask } from '../api/task';
import { isAuthenticated } from '../api/auth';
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

  // Check authentication on component mount
  useEffect(() => {
    if (!isAuthenticated()) {
      navigate('/login', { state: { from: '/create-task' } });
    }
  }, [navigate]);

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
      
      // Format the task data properly
      const taskData = {
        title: formData.title.trim(),
        description: formData.description.trim(),
        deadline: formData.deadline ? new Date(formData.deadline).toISOString() : null,
        isPriority: formData.isPriority,
        isCompleted: formData.isCompleted
        // createdAt is handled by backend
      };

      console.log('Submitting task data:', taskData); // Debug log
      
      const response = await createTask(taskData);
      console.log('Task created successfully:', response);
      
      navigate('/', { 
        state: { 
          message: 'Task created successfully!',
          newTask: response.data
        }
      });
      
    } catch (err) {
      console.error('Full error details:', err);
      setError(
        err.message || 
        err.response?.data?.message || 
        'Failed to create task. Please try again.'
      );
      
      // If unauthorized, redirect to login
      if (err.response?.status === 401) {
        navigate('/login', { state: { from: '/create-task' } });
      }
    } finally {
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
              <button 
                onClick={() => setError(null)} 
                className="error-close"
                aria-label="Close error"
              >
                &times;
              </button>
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
              aria-describedby="title-help"
            />
            <small id="title-help" className="help-text">
              Maximum 100 characters
            </small>
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
              aria-describedby="desc-help"
            />
            <small id="desc-help" className="help-text">
              Maximum 500 characters
            </small>
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
              min={new Date().toISOString().slice(0, 16)} // Prevent past dates
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
              aria-busy={loading}
            >
              {loading ? (
                <>
                  <span className="spinner" aria-hidden="true"></span>
                  Creating...
                </>
              ) : 'Create Task'}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}

export default CreateTaskPage;