import './App.css';

import {
    createBrowserRouter,
    RouterProvider
} from 'react-router-dom';
import Home from './pages/Home.tsx';
import Login from './pages/Login.tsx';
import Profile from './pages/Profile.tsx';
import Register from './pages/Register.tsx';
import About from './pages/About.tsx';
import Logout from './components/Logout.tsx';
import ProtectedRoute from './components/ProtectedRoute.tsx';
import Assets from './pages/Assets.tsx';
import Textures from './pages/Textures.tsx';
import { AuthContext, useAuth } from './hooks/useAuth.tsx';

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
        path: '/assets',
        element: <Assets />,
    },
    {
        path: '/textures',
        element: <Textures />,
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