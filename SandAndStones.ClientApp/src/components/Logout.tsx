import { useEffect, } from 'react';
import { Navigate, useNavigate } from 'react-router-dom';
import { axiosInstance } from '../hooks/useAxios';
import { useAuth } from '../context/AuthProvider';

interface AuthObjectSet {
    setContextToken: (newToken: string | null) => void
}

function Logout() {
    const { setContextToken } = useAuth() as AuthObjectSet;

    const navigate = useNavigate();
    const handleLogout = () => {
        axiosInstance.get(`${import.meta.env.VITE_APP_BASE_URL}/userAuthorization/logout`)
            .then((response) => {
            console.log(response.data);
            if (response.status == 200) {
                setContextToken(null);
                navigate('/', { replace: true });
                console.info("Logout successful.");
            }
            else {
                console.error("Logout error. Details: " + response.status + " " + response.statusText);
            }
        })
        .catch((error) => {
            console.error(error);
            console.error("Logout error");
        });
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