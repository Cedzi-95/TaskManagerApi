import React, { useState, useEffect } from 'react';
import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import HomePage from './pages/homePage.Jsx';
import CreateTaskPage from './pages/createTaskPage';
import ViewTasksPage from "./pages/viewTasks";
import { LoginPage, RegisterPage } from "./pages/authenticationPage";
import Navbar from './component/navbar';
import './App.css';

function App() {
  const [user, setUser] = useState(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    // Check if user is logged in on app load
    const token = localStorage.getItem('authToken');
    const userData = localStorage.getItem('userData');
    
    if (token && userData) {
      setUser(JSON.parse(userData));
    }
    setLoading(false);
  }, []);

  const handleLogin = (userData, token) => {
    setUser(userData);
    localStorage.setItem('authToken', token);
    localStorage.setItem('userData', JSON.stringify(userData));
  };

  const handleLogout = () => {
    setUser(null);
    localStorage.removeItem('authToken');
    localStorage.removeItem('userData');
  };

  if (loading) {
    return <div className="loading">Loading...</div>;
  }

  return (
    <Router>
      <div className="App">
        {user && <Navbar user={user} onLogout={handleLogout} />}
        <main className="main-content">
          <Routes>
            {/* Public Routes */}
            <Route 
              path="/login" 
              element={!user ? <LoginPage onLogin={handleLogin} /> : <Navigate to="/" />} 
            />
            <Route 
              path="/register" 
              element={!user ? <RegisterPage onLogin={handleLogin} /> : <Navigate to="/" />} 
            />
            
            {/* Protected Routes */}
            <Route 
              path="/" 
              element={user ? <HomePage user={user} /> : <Navigate to="/login" />} 
            />
            <Route 
              path="/create-task" 
              element={user ? <CreateTaskPage user={user} /> : <Navigate to="/login" />} 
            />
            <Route 
              path="/tasks" 
              element={user ? <ViewTasksPage user={user} /> : <Navigate to="/login" />} 
            />
            <Route 
              path="/task/:id" 
              element={user ? <ViewTasksPage user={user} /> : <Navigate to="/login" />} 
            />
            
            {/* Catch all route */}
            <Route path="*" element={<Navigate to={user ? "/" : "/login"} />} />
          </Routes>
        </main>
      </div>
    </Router>
  );
}

export default App;