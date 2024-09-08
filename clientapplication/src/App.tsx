import './App.css';

import {
    createBrowserRouter,
    createRoutesFromElements,
    Outlet,
    Route,
    RouterProvider
} from 'react-router-dom';
import Home from './pages/Home.tsx';
import Login from './pages/Login.tsx';
import Register from './pages/Register.tsx';
import About from './pages/About.tsx';
import Navbar from './components/Navbar.tsx';

function App() {
    const router = createBrowserRouter(
        createRoutesFromElements(
            <Route path="/" element={<AppContent />}>
                <Route path="/home" element={<Home />} />
                <Route path="/login" element={<Login />} />
                <Route path="/register" element={<Register />} />
                <Route path="/about" element={<About />} />
            </Route>
        )
    );

    return (
        <div className="App">
            <RouterProvider router={router} />
        </div>
    );

};

export default App;

const AppContent = () => {
    return (
        <>
            <h1>
                SandAndStonesDev
            </h1>
            <div>
                <Navbar />
            </div>
            <div>
                <Outlet />
            </div>
        </>
    );
};