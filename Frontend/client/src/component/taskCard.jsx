import React, { useState } from 'react';
import { Link } from 'react-router-dom';
import { formatDatePretty } from '../utilities/date';
import { updateTask, deleteTask, completeTask } from '../api/task';
import '../styles/task.css';

export default function TaskCard({ task, onTaskUpdated, onTaskDeleted, showActions = false }) {
  const [isUpdating, setIsUpdating] = useState(false);
  const [showMenu, setShowMenu] = useState(false);
  const [currentTask, setCurrentTask] = useState(task);

  const handleToggleComplete = async () => {
    setIsUpdating(true);
    try {
      const updatedTask = await completeTask(currentTask.id);
      setCurrentTask(updatedTask);
      if (onTaskUpdated) onTaskUpdated(updatedTask);
    } catch (error) {
      console.error('Failed to update task:', error);
    } finally {
      setIsUpdating(false);
    }
  };

  const handleTogglePriority = async () => {
    setIsUpdating(true);
    try {
      const updatedTask = await updateTask(currentTask.id, { 
        ...currentTask,
        isPriority: !currentTask.isPriority 
      });
      setCurrentTask(updatedTask);
      if (onTaskUpdated) onTaskUpdated(updatedTask);
    } catch (error) {
      console.error('Failed to update task:', error);
    } finally {
      setIsUpdating(false);
      setShowMenu(false);
    }
  };

  const handleDelete = async () => {
    if (window.confirm('Are you sure you want to delete this task?')) {
      try {
        await deleteTask(currentTask.id);
        if (onTaskDeleted) onTaskDeleted(currentTask.id);
      } catch (error) {
        console.error('Failed to delete task:', error);
      }
    }
    setShowMenu(false);
  };

  const getDeadlineStatus = () => {
    if (!currentTask.deadline) return null;
    
    const deadline = new Date(currentTask.deadline);
    const now = new Date();
    const diffTime = deadline - now;
    const diffDays = Math.ceil(diffTime / (1000 * 60 * 60 * 24));
    
    if (diffDays < 0) return 'overdue';
    if (diffDays === 0) return 'due-today';
    if (diffDays <= 3) return 'due-soon';
    return 'upcoming';
  };

  const deadlineStatus = getDeadlineStatus();

  return (
    <div className={`task-card ${currentTask.isCompleted ? 'completed' : ''} ${isUpdating ? 'updating' : ''}`}>
      <Link className="task-card-link" to={`/task/${currentTask.id}`}>
        <div className="task-card-content">
          {/* Header */}
          <div className="task-card-header">
            <h3 className="task-title">{currentTask.title}</h3>
            <div className="task-badges">
              {currentTask.isPriority && (
                <span className="priority-badge">Priority</span>
              )}
              {currentTask.isCompleted && (
                <span className="completed-badge">Completed</span>
              )}
            </div>
          </div>

          {/* Description */}
          {currentTask.description && (
            <div className="task-description">
              <p>{currentTask.description}</p>
            </div>
          )}

          {/* Footer */}
          <div className="task-card-footer">
            <div className="task-dates">
              <span className="created-date">
                Created: {formatDatePretty(currentTask.createdAt)}
              </span>
              {currentTask.deadline && (
                <span className={`deadline-date ${deadlineStatus}`}>
                  Due: {formatDatePretty(currentTask.deadline)}
                </span>
              )}
            </div>
          </div>
        </div>
      </Link>

      {/* Action Menu */}
      {showActions && (
        <div className="task-actions">
          <button
            className="action-menu-btn"
            onClick={(e) => {
              e.preventDefault();
              setShowMenu(!showMenu);
            }}
            disabled={isUpdating}
          >
            ⋮
          </button>

          {showMenu && (
            <div className="action-menu">
              <button
                className="action-item"
                onClick={(e) => {
                  e.preventDefault();
                  handleToggleComplete();
                }}
                disabled={isUpdating}
              >
                {currentTask.isCompleted ? '↶ Mark Incomplete' : '✓ Mark Complete'}
              </button>
              
              <button
                className="action-item"
                onClick={(e) => {
                  e.preventDefault();
                  handleTogglePriority();
                }}
                disabled={isUpdating}
              >
                {currentTask.isPriority ? '★ Remove Priority' : '☆ Mark Priority'}
              </button>
              
              <hr className="action-divider" />
              
              <button
                className="action-item delete"
                onClick={(e) => {
                  e.preventDefault();
                  handleDelete();
                }}
                disabled={isUpdating}
              >
                🗑 Delete Task
              </button>
            </div>
          )}
        </div>
      )}

      {/* Quick Complete Toggle */}
      {showActions && (
        <div className="quick-actions">
          <button
            className={`quick-complete-btn ${currentTask.isCompleted ? 'completed' : ''}`}
            onClick={(e) => {
              e.preventDefault();
              handleToggleComplete();
            }}
            disabled={isUpdating}
            title={currentTask.isCompleted ? 'Mark as incomplete' : 'Mark as complete'}
          >
            {currentTask.isCompleted ? '✓' : '○'}
          </button>
        </div>
      )}

      {/* Click outside to close menu */}
      {showMenu && (
        <div 
          className="menu-overlay"
          onClick={() => setShowMenu(false)}
        />
      )}
    </div>
  );
}