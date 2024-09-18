import Image from "../assets/SandAndStonesEngineScreen.png";
import Navbar from "../components/Navbar";


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
                    <img className="img-fluid" style={sectionStyle} src={Image}></img>
                </div>
            </div>
        </>
  );
}

export default About;