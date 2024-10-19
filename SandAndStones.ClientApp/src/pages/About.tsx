import Navbar from "../components/Navbar";
import ScreenImage from "../images/SandAndStonesEngineScreen.png";

const sectionStyle = {
    width: 400,
    height: 400
};


function About() {
    return (
        <>
            <Navbar />
            <p>SandAndStonesEngine Screen</p>
            <div className="row">
                <div className="logo">
                    <img className="img-fluid" style={sectionStyle} src={ScreenImage}></img>
                </div>
            </div>
        </>
  );
}

export default About;