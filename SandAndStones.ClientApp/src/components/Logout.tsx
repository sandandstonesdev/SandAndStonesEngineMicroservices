import { useEffect, } from 'react';
import { Navigate, useNavigate } from 'react-router-dom';
import { useAuth } from '../hooks/useAuth';

function Logout() {
    const auth = useAuth();

    const navigate = useNavigate();
    const handleLogout = () => {
        fetch("/userauthorization/logout").then((data) => {
            if (data.ok) {
                    auth.removeUser()
                    navigate('/login', { replace: true });
                }
                else {
                    console.error("Logout error");
                }
            })
            .catch((error) => {
                console.error(error);
            })

    };
    
    useEffect(() => {
        handleLogout()
    });

    return (
        <>
            <Navigate to="/login" replace={true} />
        </>);
}

export default Logout;