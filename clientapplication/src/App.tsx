import './App.css';

import {
    createBrowserRouter,
    RouterProvider
} from 'react-router-dom';
import Home from './pages/Home.tsx';
import Login from './pages/Login.tsx';
import Register from './pages/Register.tsx';
import About from './pages/About.tsx';
import Logout from './components/Logout.tsx';
import Profile from './components/Profile.tsx';
import ProtectedRoute from './components/ProtectedRoute.tsx';
import { AuthContext, useAuth } from './components/AuthProvider.tsx';

const router = createBrowserRouter([
    {
        path: '/',
        element: (
            <ProtectedRoute>
                <Home/>
            </ProtectedRoute>
        ),
    },
    {
        path: '/login',
        element: <Login />,
    },
    {
        path: '/about',
        element: <About />,
    },
    {
        path: '/register',
        element: <Register />,
    },
    {
        path: '/profile',
        element: <Profile />,
    },
    {
        path: '/logout',
        element: <Logout />,
    },
]);

function App() {
    const { user, setUser } = useAuth();
    
    return (
        <AuthContext.Provider value={{ user, setUser }}>
            <RouterProvider router={router} />
        </AuthContext.Provider>
    );
};

export default App;