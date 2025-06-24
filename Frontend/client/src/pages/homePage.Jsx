import React, { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import TaskCard from '../component/taskCard';
import { getTasks } from '../api/task';
import '../styles/homePage.css';

function HomePage({ user }) {
  const [tasks, setTasks] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [stats, setStats] = useState({
    total: 0,
    completed: 0,
    pending: 0,
    priority: 0
  });

  useEffect(() => {
    loadDashboardData();
  }, []);

  const loadDashboardData = async () => {
    try {
      setLoading(true);
      const response = await getTasks();
      const allTasks = response.data || [];
      
      // Get recent tasks (last 5)
      const recentTasks = allTasks.slice(0, 5);
      setTasks(recentTasks);
      
      // Calculate stats
      const stats = {
        total: allTasks.length,
        completed: allTasks.filter(task => task.isCompleted).length,
        pending: allTasks.filter(task => !task.isCompleted).length,
        priority: allTasks.filter(task => task.isPriority).length
      };
      setStats(stats);
      
      setError(null);
    } catch (err) {
      setError('Failed to load dashboard data');
      console.error('Dashboard error:', err);
    } finally {
      setLoading(false);
    }
  };

  if (loading) {
    return <div className="loading">Loading dashboard...</div>;
  }

  if (error) {
    return (
      <div className="error-container">
        <div className="error-message">{error}</div>
        <button onClick={loadDashboardData} className="retry-btn">
          Try Again
        </button>
      </div>
    );
  }

  return (
    <div className="homepage">
      <div className="homepage-header">
        <h1>Welcome back, {user.username}!</h1>
        <p>Here's what's happening with your tasks today.</p>
      </div>

      {/* Dashboard Stats */}
      <div className="dashboard-stats">
        <div className="stat-card">
          <div className="stat-number">{stats.total}</div>
          <div className="stat-label">Total Tasks</div>
        </div>
        <div className="stat-card">
          <div className="stat-number">{stats.pending}</div>
          <div className="stat-label">Pending</div>
        </div>
        <div className="stat-card">
          <div className="stat-number">{stats.completed}</div>
          <div className="stat-label">Completed</div>
        </div>
        <div className="stat-card priority">
          <div className="stat-number">{stats.priority}</div>
          <div className="stat-label">Priority</div>
        </div>
      </div>

      {/* Quick Actions */}
      <div className="quick-actions">
        <h2>Quick Actions</h2>
        <div className="action-buttons">
          <Link to="/create-task" className="action-btn primary">
            + Create New Task
          </Link>
          <Link to="/tasks" className="action-btn secondary">
            View All Tasks
          </Link>
        </div>
      </div>

      {/* Recent Tasks */}
      <div className="recent-tasks">
        <div className="section-header">
          <h2>Recent Tasks</h2>
          <Link to="/tasks" className="view-all-link">View All</Link>
        </div>
        
        {tasks.length === 0 ? (
          <div className="empty-state">
            <h3>No tasks yet</h3>
            <p>Create your first task to get started!</p>
            <Link to="/create-task" className="create-first-task-btn">
              Create Task
            </Link>
          </div>
        ) : (
          <div className="tasks-grid">
            {tasks.map(task => (
              <TaskCard key={task.id} task={task} />
            ))}
          </div>
        )}
      </div>
    </div>
  );
}

export default HomePage;