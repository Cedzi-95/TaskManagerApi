import React, { useState, useEffect } from 'react';
import { Link, useParams } from 'react-router-dom';
import TaskCard from '../component/taskCard';
import { getTasks, deleteTask, updateTask } from '../api/task';
import '../styles/viewTasksPage.css';

function ViewTasksPage({ user }) {
  const { id } = useParams(); // For viewing single task
  const [tasks, setTasks] = useState([]);
  const [filteredTasks, setFilteredTasks] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [filters, setFilters] = useState({
    search: '',
    status: 'all', // all, completed, pending
    priority: 'all', // all, priority, normal
    sortBy: 'createdAt' // createdAt, deadline, title
  });

  useEffect(() => {
    loadTasks();
  }, []);

  useEffect(() => {
    applyFilters();
  }, [tasks, filters]);

  const loadTasks = async () => {
    try {
      setLoading(true);
      const response = await getTasks();
      setTasks(response.data || []);
      setError(null);
    } catch (err) {
      setError('Failed to load tasks');
      console.error('Load tasks error:', err);
    } finally {
      setLoading(false);
    }
  };

  const applyFilters = () => {
    let filtered = [...tasks];

    // Search filter
    if (filters.search) {
      const searchLower = filters.search.toLowerCase();
      filtered = filtered.filter(task =>
        task.title.toLowerCase().includes(searchLower) ||
        task.description.toLowerCase().includes(searchLower)
      );
    }

    // Status filter
    if (filters.status !== 'all') {
      filtered = filtered.filter(task =>
        filters.status === 'completed' ? task.isCompleted : !task.isCompleted
      );
    }

    // Priority filter
    if (filters.priority !== 'all') {
      filtered = filtered.filter(task =>
        filters.priority === 'priority' ? task.isPriority : !task.isPriority
      );
    }

    // Sort
    filtered.sort((a, b) => {
      switch (filters.sortBy) {
        case 'title':
          return a.title.localeCompare(b.title);
        case 'deadline':
          if (!a.deadline && !b.deadline) return 0;
          if (!a.deadline) return 1;
          if (!b.deadline) return -1;
          return new Date(a.deadline) - new Date(b.deadline);
        case 'createdAt':
        default:
          return new Date(b.createdAt) - new Date(a.createdAt);
      }
    });

    setFilteredTasks(filtered);
  };

  const handleFilterChange = (filterType, value) => {
    setFilters(prev => ({
      ...prev,
      [filterType]: value
    }));
  };

  const handleTaskUpdate = async (taskId, updates) => {
    try {
      await updateTask(taskId, updates);
      setTasks(prev => prev.map(task =>
        task.id === taskId ? { ...task, ...updates } : task
      ));
    } catch (err) {
      setError('Failed to update task');
      console.error('Update task error:', err);
    }
  };

  const handleTaskDelete = async (taskId) => {
    if (!window.confirm('Are you sure you want to delete this task?')) {
      return;
    }

    try {
      await deleteTask(taskId);
      setTasks(prev => prev.filter(task => task.id !== taskId));
    } catch (err) {
      setError('Failed to delete task');
      console.error('Delete task error:', err);
    }
  };

  const clearFilters = () => {
    setFilters({
      search: '',
      status: 'all',
      priority: 'all',
      sortBy: 'createdAt'
    });
  };

  if (loading) {
    return <div className="loading">Loading tasks...</div>;
  }

  return (
    <div className="view-tasks-page">
      <div className="page-header">
        <h1>My Tasks</h1>
        <div className="header-actions">
          <Link to="/create-task" className="btn-create-task">
            + New Task
          </Link>
        </div>
      </div>

      {error && (
        <div className="error-alert">
          {error}
          <button onClick={loadTasks} className="retry-btn">Retry</button>
        </div>
      )}

      {/* Filters */}
      <div className="filters-container">
        <div className="search-filter">
          <input
            type="text"
            placeholder="Search tasks..."
            value={filters.search}
            onChange={(e) => handleFilterChange('search', e.target.value)}
            className="search-input"
          />
        </div>

        <div className="filter-controls">
          <select
            value={filters.status}
            onChange={(e) => handleFilterChange('status', e.target.value)}
            className="filter-select"
          >
            <option value="all">All Status</option>
            <option value="pending">Pending</option>
            <option value="completed">Completed</option>
          </select>

          <select
            value={filters.priority}
            onChange={(e) => handleFilterChange('priority', e.target.value)}
            className="filter-select"
          >
            <option value="all">All Priority</option>
            <option value="priority">Priority</option>
            <option value="normal">Normal</option>
          </select>

          <select
            value={filters.sortBy}
            onChange={(e) => handleFilterChange('sortBy', e.target.value)}
            className="filter-select"
          >
            <option value="createdAt">Sort by Created</option>
            <option value="deadline">Sort by Deadline</option>
            <option value="title">Sort by Title</option>
          </select>

          <button onClick={clearFilters} className="clear-filters-btn">
            Clear Filters
          </button>
        </div>
      </div>

      {/* Task Stats */}
      <div className="task-stats">
        <span>Showing {filteredTasks.length} of {tasks.length} tasks</span>
      </div>

      {/* Tasks Grid */}
      <div className="tasks-container">
        {filteredTasks.length === 0 ? (
          <div className="empty-state">
            {tasks.length === 0 ? (
              <>
                <h3>No tasks found</h3>
                <p>Create your first task to get started!</p>
                <Link to="/create-task" className="btn-create-first">
                  Create Task
                </Link>
              </>
            ) : (
              <>
                <h3>No tasks match your filters</h3>
                <p>Try adjusting your search or filter criteria.</p>
                <button onClick={clearFilters} className="btn-clear-filters">
                  Clear Filters
                </button>
              </>
            )}
          </div>
        ) : (
          <div className="tasks-grid">
            {filteredTasks.map(task => (
              <TaskCard
                key={task.id}
                task={task}
                onUpdate={handleTaskUpdate}
                onDelete={handleTaskDelete}
                showActions={true}
              />
            ))}
          </div>
        )}
      </div>
    </div>
  );
}

export default ViewTasksPage;