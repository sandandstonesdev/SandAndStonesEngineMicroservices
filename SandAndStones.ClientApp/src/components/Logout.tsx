import { useEffect, } from 'react';
import { Navigate, useNavigate } from 'react-router-dom';
import { useAuth } from '../hooks/useAuth';
import { axiosInstance } from '../hooks/useAxios';

function Logout() {
    const auth = useAuth();

    const navigate = useNavigate();
    const handleLogout = () => {
        axiosInstance.get('/userAuthorization/logout')
            .then((response) => {
            console.log(response.data);
            if (response.status == 200) {
                auth.removeUser()
                navigate('/login', { replace: true });
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