%.pdf: %.tex
	pdflatex $< && pdflatex $< && pdflatex $<

all: report.pdf

clean:
	rm *.aux *.snm *.toc *.log