import { useEffect, } from 'react';
import { useNavigate } from "react-router-dom";
import { Navigate } from 'react-router-dom';

function Logout() {
    const navigate = useNavigate();

    const handleLogout = () => {
        fetch("/logout", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            body: ""

        }).then((data) => {
                if (data.ok) {

                    navigate("/login", { replace: true });
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
    }, []);

    return <Navigate to="/login"></Navigate>;
}

export default Logout;