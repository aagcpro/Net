<?php
require('fpdf/fpdf.php');


class PDF extends FPDF
{
var $B;
var $I;
var $U;
var $HREF;
var $widths;
var $aligns;

function SetWidths($w)
{
	//Set the array of column widths
	$this->widths=$w;
}

function SetAligns($a)
{
	//Set the array of column alignments
	$this->aligns=$a;
}

function Row($data)
{
	//Calculate the height of the row
	$nb=0;
	for($i=0;$i<count($data);$i++)
		$nb=max($nb,$this->NbLines($this->widths[$i],$data[$i]));
	$h=2.6*$nb;
	//Issue a page break first if needed
	$this->CheckPageBreak($h);
#	$this->Encabezado();
#	$this->SetX(92.5);
#	$this->SetY(60);
	//Draw the cells of the row
	for($i=0;$i<count($data);$i++)
	{
		$w=$this->widths[$i];
		$a=isset($this->aligns[$i]) ? $this->aligns[$i] : 'L';
		//Save the current position
		$x=$this->GetX();
		$y=$this->GetY();
		//Draw the border
		
		if (strlen($data[3]) > 50)		 
		$h=5*$nb;		
		$this->Rect($x,$y,$w,$h);
#		if ($tam == '4.5')
		$this->SetFont('Arial','',5);
		if ($i > 6)
		$this->SetFont('Arial','',3.8);
		if (strlen($data[$i]) < 3)
		$this->SetFont('Arial','',5);		
		if (strlen($data[$i]) > 45)
		$this->MultiCell($w,5,$data[$i],0,$a,'false');	//  aqui cambien la linea de arriba para que no sea multilinea		
		else
		$this->Cell($w,5,$data[$i],0,$a,'false');		
#		$this->MultiCell($w,5,$data[$i],0,$a,'false');	//  aqui cambien la linea de arriba para que no sea multilinea
		//Put the position to the right of the cell
		$this->SetXY($x+$w,$y);
	}
	//Go to the next line
	$this->Ln($h);
}



function NbLines($w,$txt)
{
	//Computes the number of lines a MultiCell of width w will take
	$cw=&$this->CurrentFont['cw'];
	if($w==0)
		$w=$this->w-$this->rMargin-$this->x;
	$wmax=($w-2*$this->cMargin)*1000/$this->FontSize;
	$s=str_replace("\r",'',$txt);
	$nb=strlen($s);
	if($nb>0 and $s[$nb-1]=="\n")
		$nb--;
	$sep=-1;
	$i=0;
	$j=0;
	$l=0;
	$nl=1;
	while($i<$nb)
	{
		$c=$s[$i];
		if($c=="\n")
		{
			$i++;
			$sep=-1;
			$j=$i;
			$l=0;
			$nl++;
			continue;
		}
		if($c==' ')
			$sep=$i;
		$l+=$cw[$c];
		if($l>$wmax)
		{
			if($sep==-1)
			{
				if($i==$j)
					$i++;
			}
			else
				$i=$sep+1;
			$sep=-1;
			$j=$i;
			$l=0;
			$nl++;
		}
		else
			$i++;
	}
	return $nl;
}

#function PDF($orientation='P', $unit='mm', $size='eti')
function PDF($orientation='P', $unit='mm', $size='eti')
{
    // Call parent constructor
    $this->FPDF($orientation,$unit,$size);
    // Initialization
    $this->B = 0;
    $this->I = 0;
    $this->U = 0;
    $this->HREF = '';
}

function WriteHTML($html)
{
    // HTML parser
    $html = str_replace("\n",' ',$html);
    $a = preg_split('/<(.*)>/U',$html,-1,PREG_SPLIT_DELIM_CAPTURE);
    foreach($a as $i=>$e)
    {
        if($i%2==0)
        {
            // Text
            if($this->HREF)
                $this->PutLink($this->HREF,$e);
            else
                $this->Write(5,$e);
        }
        else
        {
            // Tag
            if($e[0]=='/')
                $this->CloseTag(strtoupper(substr($e,1)));
            else
            {
                // Extract attributes
                $a2 = explode(' ',$e);
                $tag = strtoupper(array_shift($a2));
                $attr = array();
                foreach($a2 as $v)
                {
                    if(preg_match('/([^=]*)=["\']?([^"\']*)/',$v,$a3))
                        $attr[strtoupper($a3[1])] = $a3[2];
                }
                $this->OpenTag($tag,$attr);
            }
        }
    }
}

function OpenTag($tag, $attr)
{
    // Opening tag
    if($tag=='B' || $tag=='I' || $tag=='U')
        $this->SetStyle($tag,true);
    if($tag=='A')
        $this->HREF = $attr['HREF'];
    if($tag=='BR')
        $this->Ln(5);
}

function CloseTag($tag)
{
    // Closing tag
    if($tag=='B' || $tag=='I' || $tag=='U')
        $this->SetStyle($tag,false);
    if($tag=='A')
        $this->HREF = '';
}

function SetStyle($tag, $enable)
{
    // Modify style and select corresponding font
    $this->$tag += ($enable ? 1 : -1);
    $style = '';
    foreach(array('B', 'I', 'U') as $s)
    {
        if($this->$s>0)
            $style .= $s;
    }
    $this->SetFont('',$style);
}

function PutLink($URL, $txt)
{
    // Put a hyperlink
    $this->SetTextColor(0,0,255);
    $this->SetStyle('U',true);
    $this->Write(5,$txt,$URL);
    $this->SetStyle('U',false);
    $this->SetTextColor(0);
}


}
/*
$html = 'You can now easily print text mixing different styles: <b>bold</b>, <i>italic</i>,
<u>underlined</u>, or <b><i><u>all at once</u></i></b>!<br><br>You can also insert links on
text, such as <a href="http://www.fpdf.org">www.fpdf.org</a>, or on an image: click on the logo.';
*/
require("funcionespdf.php");
$pdf = new PDF();
// First page
$pdf->AddPage();
/* //Esto crea el header original, de ahi se paso a la funcion Header() para que se muestre a cada inicio de pantalla
$pdf->Image('imapc/pdf/cm.png',8,12,37,0,'');
$pdf->Image('imapc/pdf/linea.png',45,13,.7,0,'');
$pdf->SetFont('Arial','B',6); //Seleccionamos Arial - Bold y en tamaño 6
$pdf->Text(47,15,utf8_decode("OFICIALÍA MAYOR"));
$pdf->SetFont('times','',6); //Seleccionamos Arial - Bold y en tamaño 6
$pdf->Text(47,18,utf8_decode("CAPTRALIR / Caja de Previsión para Trabajadores"));
$pdf->Text(47,21,utf8_decode("a Lista de Raya del Gobierno del Distrito Federal"));
$pdf->SetFont('Arial','B',6); //Seleccionamos Arial - Bold y en tamaño 6
$pdf->Text(47,24,utf8_decode("DIRECCIÓN GENERAL / CAPTRALIR"));
$pdf->Text(47,27,utf8_decode("DIRECCIÓN DE ADMINISTRACIÓN Y FINANZAS"));
$pdf->Text(47,30,utf8_decode("SUBDIRECCIÓN DE ADMINISTRACIÓN"));
$pdf->SetFont('Arial','',6); //Seleccionamos Arial - Bold y en tamaño 6
$pdf->Text(47,33,utf8_decode("JUD. - DE ADQUISICIONES Y SERVICIOS GENERALES / ÁREA DE INVENTARIOS"));
$pdf->SetFont('times','I',27); //Seleccionamos Arial - Bold y en tamaño 27
$pdf->Text(150,33,utf8_decode("EJERCICIO FISCAL ").date("Y"));
$pdf->Image('imapc/pdf/fonto_tabla.png',10,38,250,0,'');
$pdf->SetFont('Arial','B',14); //Seleccionamos Arial - Bold y en tamaño 14
$pdf->Text(50,44,utf8_decode("** RESGUARDO MÚLTIPLE DE BIENES MUEBLES - INSTRUMENTALES **"));
$pdf->SetFont('Arial','',10); //Seleccionamos Arial - Bold y en tamaño 10
$pdf->Text(36,48,utf8_decode("TOMA FISICA Y VALIDACIÓN DEL PADRÓN INVENTARIAL HISTÓRICO, CON CORTE AL DÍA ").strtoupper(date_es("l d") . " de " . date_es("F") . " de " . date_es("Y")));
// aqui habia otro corte del header
$pdf->SetFont('Arial','B',11); //Seleccionamos Arial - Bold y en tamaño 8
$pdf->Text(92.5,60,utf8_decode("DIRECCIÓN GENERAL DE LA CAPTRALIR"));
$pdf->SetFont('Arial','',9); //Seleccionamos Arial - Bold y en tamaño 8
$pdf->Text(95,63.5,utf8_decode("DIRECCIÓN DE ADMINISTRACIÓN DE FINANZAS"));
$pdf->Text(102,66.5,utf8_decode("SUBDIRECCIÓN DE ADMINISTRACIÓN"));
$pdf->SetFont('Arial','B',9); //Seleccionamos Arial - Bold y en tamaño 8
$pdf->Text(55,69.5,utf8_decode("** JEFATURA DE LA UNIDAD DEPARTAMENTAL DE ADQUISICIONES Y SERVICIOS GENERALES **"));
*/
require("qr/index.php");
$pdf->AddFont('league','','league.php');
#$pdf->AddFont('visitor','','visitor.php');
$pdf->SetFont('league','I',20); //Seleccionamos Arial - Bold y en tamaño 8
$pdf->Image("imapc/pdf/cmv.png",5,16,20,0,'','');
$pdf->Image($PNG_WEB_DIR.basename($filename),25,15,37,0,'','');

#	$in = explode(" ",$descripcion);
#	$cuantos = count($in);
#	$con = 7;
#	for ($x=0;$x<=$cuantos;$x++)
#	{
#		$pdf->Text(.5,$con,utf8_decode($in[$x]));
#		echo $in[$x] . "$con <br>";
#		$con = $con+6;
#	}
function espacio($a)
{
$dade = array('DA','DE','DI','DO','DU');
$deadee = array('D A','D E','D I','D O','D U');
$final = str_replace($dade,$deadee,$a);
return $final;
}
$total = strlen($descripcion);
if ($total <= 30)
	$pdf->Text(24,6,utf8_decode(espacio($descripcion)));
else
	$pdf->Text(3,6,utf8_decode(espacio($descripcion)));
$pdf->SetFont('league','I',26); //Seleccionamos Arial - Bold y en tamaño 8
$pdf->Text(34,15,utf8_decode($progresivo));
$pdf->Image("imapc/pdf/captralirv.png",63,19,10,0,'','');

#$pdf->SetFont('league','',15); //Seleccionamos Arial - Bold y en tamaño 8
#$pdf->AddPage();
// .number_format($costo,2) // este texto va a ir en la segunda hoja al final
// Second page
/*
$pdf->AddPage();
$pdf->SetLink($link);
$pdf->Image('logo.png',10,12,30,0,'','http://www.fpdf.org');
$pdf->SetLeftMargin(45);
$pdf->SetFontSize(14);
$pdf->WriteHTML($html);
$pdf->Output();
*/
$pdf->Output();
?>