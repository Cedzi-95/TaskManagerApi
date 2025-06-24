import React, { useState } from 'react';
import { Link, useLocation } from 'react-router-dom';
import '../styles/nav.css';

function Navbar({ user, onLogout }) {
  const location = useLocation();
  const [isMenuOpen, setIsMenuOpen] = useState(false);

  const isActive = (path) => {
    return location.pathname === path ? 'active' : '';
  };

  const handleLogout = () => {
    if (window.confirm('Are you sure you want to logout?')) {
      onLogout();
    }
  };

  const toggleMenu = () => {
    setIsMenuOpen(!isMenuOpen);
  };

  return (
    <nav className="navbar">
      <div className="navbar-container">
        <div className="navbar-brand">
          <Link to="/" className="brand-link">
            TaskManager
          </Link>
        </div>

        <div className={`navbar-menu ${isMenuOpen ? 'active' : ''}`}>
          <div className="navbar-nav">
            <Link
              to="/"
              className={`nav-link ${isActive('/')}`}
              onClick={() => setIsMenuOpen(false)}
            >
              Dashboard
            </Link>
            <Link
              to="/tasks"
              className={`nav-link ${isActive('/tasks')}`}
              onClick={() => setIsMenuOpen(false)}
            >
              All Tasks
            </Link>
            <Link
              to="/create-task"
              className={`nav-link ${isActive('/create-task')}`}
              onClick={() => setIsMenuOpen(false)}
            >
              Create Task
            </Link>
          </div>

          <div className="navbar-user">
            <div className="user-info">
              <span className="user-name">
                {user.username || user.email}
              </span>
            </div>
            <button
              onClick={handleLogout}
              className="logout-btn"
            >
              Logout
            </button>
          </div>
        </div>

        <button
          className="navbar-toggle"
          onClick={toggleMenu}
          aria-label="Toggle menu"
        >
          <span></span>
          <span></span>
          <span></span>
        </button>
      </div>
    </nav>
  );
}

export default Navbar;