import { Container, CssBaseline, ThemeProvider, createTheme } from '@mui/material';
import { useState } from "react";
import Header from "./Header";
import { Outlet } from 'react-router-dom';
import { ToastContainer } from 'react-toastify';
import 'react-toastify/ReactToastify.css';
function App() {
  const [darkMode, setDarkMode] = useState(false);
  const paletteType = darkMode ? "dark" : "light";
  const theme = createTheme({
    palette: {
      mode: paletteType,
      background: {
        default: paletteType === 'light' ? '#eaeaea' : '#121212'
      }
    },
  });
function handleThemeChange() {
  setDarkMode (!darkMode);
}
  return (
        <div>
          <ThemeProvider theme ={theme}>
            <ToastContainer position='bottom-right' hideProgressBar theme='colored'/>
             <CssBaseline/>
             <Header darkMode={darkMode} handleThemeChange={handleThemeChange}/>
             <Container>
              <Outlet/>
             </Container>
          </ThemeProvider>
        </div>
  ); 
}
export default App;
