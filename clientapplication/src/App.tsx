import './App.css';

import {
    createBrowserRouter,
    createRoutesFromElements,
    Route,
    RouterProvider
} from 'react-router-dom';
import Home from './pages/Home.tsx';
import Login from './pages/Login.tsx';
import Register from './pages/Register.tsx';
import About from './pages/About.tsx';
import NoMatch from './components/NoMatch.tsx';
import Logout from './components/Logout.tsx';
import Profile from './components/Profile.tsx';

function App() {
    const router = createBrowserRouter(
        createRoutesFromElements(
            <Route path="/" element={<AppContent />}>
                <Route path="/home" element={<Home />} />
                <Route path="/about" element={<About />} />
                <Route path="/login" element={<Login />} />
                <Route path="/register" element={<Register />} />
                <Route path="/profile" element={<Profile />} />
                <Route path="/logout" element={<Logout />} />
                <Route path="*" element={<NoMatch />} />
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
            <Home />
        </>
    );
};